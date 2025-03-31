using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

using System;

public class PaymentService:IPaymentRespository
{
    private readonly SalesDbContext _context;

    public PaymentService(SalesDbContext context)
    {
        _context = context;
    }

    // 🔹 Xử lý thanh toán đơn hàng
    public async Task<Payment> ProcessPaymentAsync(Guid orderId, PaymentMethodEnum paymentMethod)
    {
        //var order = await _context.Orders.FindAsync(orderId);

        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        if (order == null)
            throw new Exception("Order không tồn tại!");

        if (order.Status != OrderStatusEnum.Pending)
            throw new Exception("Order không ở trạng thái chờ xử lý!");

        // 🔹 Xử lý thanh toán (giả lập)
        var payment = new Payment
        {
            OrderId = orderId,
            PaymentMethod = paymentMethod,
            PaymentStatus = PaymentStatusEnum.Completed, // Giả lập thanh toán thành công
            Amount = order.TotalPrice,
            TransactionId = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);

        // 🔹 Cập nhật trạng thái đơn hàng
        order.Status = OrderStatusEnum.Completed;

        await _context.SaveChangesAsync();

        return payment;
    }
}
