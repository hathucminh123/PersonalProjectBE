using Microsoft.AspNetCore.Mvc;
using SalesProject.Interface;
using SalesProject.Models.Domain;
using SalesProject.Models.DTOs.Response;

namespace SalesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IPaymentRespository _paymentService;

        public PaymentController(IVnPayService vnPayService,IPaymentRespository paymentService)
        {
            _vnPayService = vnPayService;
            _paymentService = paymentService;
        }

        [HttpPost("vnpay/create")]
        public async Task<IActionResult> CreateVnPayPayment([FromQuery] Guid orderId)
        {
            try
            {
                var baseReturnUrl = $"{Request.Scheme}://{Request.Host}/api/payment/PaymentCallbackVnpay";
                var result = await _paymentService.CreateVnPayPaymentAsync(orderId, baseReturnUrl);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Tạo URL thanh toán VNPay và redirect người dùng
        /// </summary>
        [HttpPost]
        public IActionResult CreatePaymentUrlVnpay([FromBody] PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Ok(new { paymentUrl = url }); // dùng Ok để test dễ
        }

        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            return new JsonResult(response);
        }
    }
}
