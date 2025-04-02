using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Helpers;
using SalesProject.Interface;
using SalesProject.Models.Domain;

using System;

public class PaymentService : IPaymentRespository
{
    private readonly SalesDbContext _context;
    private readonly IConfiguration _config;

    public PaymentService(SalesDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<VnPayCreatePaymentResult> CreateVnPayPaymentAsync(Guid orderId, string baseReturnUrl)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId)
                    ?? throw new Exception("Đơn hàng không tồn tại!");

        if (order.Status != OrderStatusEnum.Pending)
            throw new Exception("Đơn hàng không ở trạng thái chờ thanh toán!");

        var payment = new Payment
        {
            OrderId = order.Id,
            PaymentMethod = PaymentMethodEnum.VNPay,
            PaymentStatus = PaymentStatusEnum.Pending,
            Amount = order.TotalPrice,
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
    
        string vnp_TmnCode = _config["VnPay:TmnCode"];
        string vnp_HashSecret = _config["VnPay:HashSecret"];
        string vnp_Url = _config["VnPay:BaseUrl"];
        string returnUrl = $"{baseReturnUrl}?paymentId={payment.Id}";

        var vnpParams = new Dictionary<string, string>
        {
            { "vnp_Version", "2.1.0" },
            { "vnp_Command", "pay" },
            { "vnp_TmnCode", vnp_TmnCode },
            { "vnp_Amount", ((int)(payment.Amount * 100)).ToString() },
            { "vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss") },
            { "vnp_CurrCode", "VND" },
            { "vnp_IpAddr", "127.0.0.1" }, // có thể truyền từ request
            { "vnp_Locale", "vn" },
            { "vnp_OrderInfo", $"Thanh toan don hang {order.OrderCode}" },
            { "vnp_OrderType", "other" },
            { "vnp_ReturnUrl", returnUrl },
            { "vnp_TxnRef", payment.Id.ToString() }
        };

        var paymentUrl = VnPayHelper.CreateRequestUrl(vnpParams, vnp_HashSecret, vnp_Url);

        return new VnPayCreatePaymentResult
        {
            PaymentId = payment.Id,
            PaymentUrl = paymentUrl
        };
    }

    public async Task<bool> HandleVnPayReturnAsync(IQueryCollection query)
    {
        string hashSecret = _config["VnPay:HashSecret"];

        if (!VnPayHelper.ValidateVnpaySignature(query, hashSecret))
            return false;

        Guid paymentId = Guid.Parse(query["vnp_TxnRef"]);
        var payment = await _context.Payments.Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.Id == paymentId);

        if (payment == null)
            return false;

        string responseCode = query["vnp_ResponseCode"];

        if (responseCode == "00")
        {
            payment.PaymentStatus = PaymentStatusEnum.Completed;
            payment.TransactionId = query["vnp_TransactionNo"];
            payment.Order.Status = OrderStatusEnum.Completed;
        }
        else
        {
            payment.PaymentStatus = PaymentStatusEnum.Failed;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Payment> CreateCashOnDeliveryPaymentAsync(Guid orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
            throw new Exception("Đơn hàng không tồn tại!");

        if (order.Status != OrderStatusEnum.Pending)
            throw new Exception("Đơn hàng không ở trạng thái chờ xử lý!");

        // Kiểm tra nếu đã có thanh toán COD trước đó
        var existingPayment = await _context.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId && p.PaymentMethod == PaymentMethodEnum.CashOnDelivery);

        if (existingPayment != null)
            return existingPayment;

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            PaymentMethod = PaymentMethodEnum.CashOnDelivery,
            PaymentStatus = PaymentStatusEnum.Pending, // sẽ cập nhật thành Completed sau khi giao hàng
            Amount = order.TotalPrice,
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return payment;
    }

}
