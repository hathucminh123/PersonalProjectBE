using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesProject.Dtos;
using SalesProject.Models.Domain;
namespace SalesProject.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly IMapper _mapper;

    public PaymentController(PaymentService paymentService, IMapper mapper)
    {
        _paymentService = paymentService;
        _mapper = mapper;
    }

    // 🔹 Xử lý thanh toán cho đơn hàng
    [HttpPost("pay")]
    public async Task<IActionResult> Pay(Guid orderId, PaymentMethodEnum paymentMethod)
    {
        try
        {
            var payment = await _paymentService.ProcessPaymentAsync(orderId, paymentMethod);

            // ✅ Dùng AutoMapper để ánh xạ từ Domain sang DTO
            var paymentDto = _mapper.Map<PaymentDto>(payment);

            // ✅ Trả về DTO đã được ánh xạ
            return Ok(paymentDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
