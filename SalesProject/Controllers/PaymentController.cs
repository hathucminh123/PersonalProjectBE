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

        public PaymentController(IVnPayService vnPayService, IPaymentRespository paymentService)
        {
            _vnPayService = vnPayService;
            _paymentService = paymentService;
        }

        /// <summary>
        /// Tạo URL thanh toán VNPay và trả về để frontend redirect
        /// </summary>
        [HttpPost("vnpay/create")]
        public async Task<IActionResult> CreatePaymentUrlVnpay([FromBody] PaymentInformationModel model)
        {
            try
            {
                var url = await _vnPayService.CreatePaymentUrl(model, HttpContext);
                return Ok(new { paymentUrl = url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("PaymentCallbackVnpay")]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            var result = await _vnPayService.PaymentExecuteAsync(Request.Query);

            if (result.Success)
                return Redirect($"http://localhost:5173/User/info?");

            return Redirect("/payment-failed");
        }
        //public IActionResult PaymentCallbackVnpay()
        //{
        //    var response = _vnPayService.PaymentExecuteAsync(Request.Query);
        //    return new JsonResult(response);
        //}


        /// <summary>
        /// Callback từ VNPay (redirect sau khi thanh toán)
        /// </summary>
        //[HttpGet]
        //public async Task<IActionResult> PaymentCallbackVnpay()
        //{
        //    var result = await _vnPayService.PaymentExecuteAsync(Request.Query);

        //    if (result.Success)
        //    {
        //        // 👉 Redirect đến FE trang thành công
        //        return Redirect($"/payment-success?orderId={result.OrderId}");
        //    }
        //    else
        //    {
        //        // 👉 Redirect đến FE trang thất bại
        //        return Redirect("/payment-failed");
        //    }
        //}
    }
}
