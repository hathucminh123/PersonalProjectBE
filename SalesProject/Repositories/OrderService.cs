using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

using System;
namespace SalesProject.Repositories;

public class OrderService : IOrdersRepository
{
    private readonly SalesDbContext _context;

    public OrderService(SalesDbContext context)
    {
        _context = context;
    }

    // 🔹 Tạo đơn hàng từ giỏ hàng của người dùng
    //public async Task<Orders> CreateOrderAsync(Guid userId, string? discountCode = null)
    //{
    //    var cartItems = await _context.Carts
    //        .Where(c => c.UserId == userId && (c.IsActive ?? false))
    //        .Include(c => c.Product)
    //        .ToListAsync();

    //    if (cartItems == null || !cartItems.Any())
    //        throw new Exception("Giỏ hàng trống!");

    //    decimal totalPrice = cartItems.Sum(c => (c.Product?.FinalPrice ?? 0) * c.Quantity);

    //    decimal discountAmount = 0;

    //    // 🔹 Xử lý mã giảm giá nếu có
    //    if (!string.IsNullOrEmpty(discountCode))
    //    {
    //        var discount = await _context.Discounts
    //            .Where(d => d.Code == discountCode && d.IsActive && d.ExpiryDate >= DateTime.UtcNow)
    //            .FirstOrDefaultAsync();

    //        if (discount != null)
    //        {
    //            if (discount.DiscountType == DiscountTypeEnum.Percentage)
    //            {
    //                discountAmount = totalPrice * (discount.DiscountAmount / 100);
    //            }
    //            else
    //            {
    //                discountAmount = discount.DiscountAmount;
    //            }

    //            // Giảm tổng giá trị đơn hàng
    //            totalPrice -= discountAmount;

    //            // Áp dụng giảm giá cho đơn hàng
    //            _context.OrderDiscounts.Add(new OrderDiscounts
    //            {
    //                OrderId = Guid.NewGuid(),
    //                DiscountId = discount.Id
    //            });
    //        }
    //    }

    //    // 🔹 Tạo đơn hàng
    //    var order = new Orders
    //    {
    //        UserId = userId,
    //        TotalPrice = totalPrice,
    //        DiscountAmount = discountAmount,
    //        Status = OrderStatusEnum.Pending,
    //        CreatedAt = DateTime.UtcNow
    //    };

    //    _context.Orders.Add(order);
    //    await _context.SaveChangesAsync();

    //    // 🔹 Lưu chi tiết đơn hàng
    //    foreach (var cartItem in cartItems)
    //    {
    //        var orderDetail = new OrderDetails
    //        {
    //            OrderId = order.Id,
    //            ProductId = cartItem.ProductId,
    //            Quantity = cartItem.Quantity,
    //            UnitPrice = cartItem?.Product?.FinalPrice ?? 0,
    //            Discount = 0
    //        };

    //        _context.OrderDetails.Add(orderDetail);
    //    }

    //    // 🔹 Xóa giỏ hàng sau khi đặt hàng thành công
    //    _context.Carts.RemoveRange(cartItems);

    //    await _context.SaveChangesAsync();

    //    return order;
    //}

    public async Task<Orders> CreateOrderAsync(
    Guid userId,
    Address shippingAddressInput,
    PaymentMethodEnum paymentMethod,
    string? discountCode = null)
    {
        // Bước 1: Lưu địa chỉ mới
        var shippingAddress = new Address
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FullName = shippingAddressInput.FullName,
            Phone = shippingAddressInput.Phone,
            Province = shippingAddressInput.Province,
            District = shippingAddressInput.District,
            Ward = shippingAddressInput.Ward,
            StreetAddress = shippingAddressInput.StreetAddress,
            Email = shippingAddressInput.Email,
            IsDefault = false
        };
        _context.Addresses.Add(shippingAddress);
        await _context.SaveChangesAsync();

        // Bước 2: Lấy giỏ hàng
        var cartItems = await _context.Carts
            .Where(c => c.UserId == userId && (c.IsActive ?? false))
            .Include(c => c.Product)
            .ToListAsync();

        if (!cartItems.Any())
            throw new Exception("Giỏ hàng trống!");

        decimal totalPrice = cartItems.Sum(c => (c.Product?.FinalPrice ?? 0) * c.Quantity);
        decimal discountAmount = 0;

        // Bước 3: Áp dụng mã giảm giá
        Discounts?discount = null;
        if (!string.IsNullOrEmpty(discountCode))
        {
            discount = await _context.Discounts
                .FirstOrDefaultAsync(d => d.Code == discountCode && d.IsActive && d.ExpiryDate >= DateTime.UtcNow);

            if (discount != null)
            {
                discountAmount = discount.DiscountType == DiscountTypeEnum.Percentage
                    ? totalPrice * (discount.DiscountAmount / 100)
                    : discount.DiscountAmount;

                totalPrice -= discountAmount;
            }
        }

        // Bước 4: Tạo đơn hàng
        var order = new Orders
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ShippingAddressId = shippingAddress.Id,
            TotalPrice = totalPrice,
            DiscountAmount = discountAmount,
            PaymentMethod = paymentMethod,
            Status = OrderStatusEnum.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Bước 5: Tạo chi tiết đơn hàng
        foreach (var cartItem in cartItems)
        {
            var detail = new OrderDetails
            {
                OrderId = order.Id,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.Product?.FinalPrice ?? 0
            };
            _context.OrderDetails.Add(detail);

            // Giảm tồn kho nếu cần
            cartItem.Product.Stock -= cartItem.Quantity;
        }

        // Bước 6: Gắn mã giảm giá
        if (discount != null)
        {
            _context.OrderDiscounts.Add(new OrderDiscounts
            {
                OrderId = order.Id,
                DiscountId = discount.Id
            });
        }

        // Bước 7: Xoá giỏ hàng
        _context.Carts.RemoveRange(cartItems);

        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<List<Orders>> GetOrdersHistory()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderDetails) // Không cần kiểm tra null
                .ThenInclude(od => od.Product)
            .Include(o => o.OrderDiscounts)
                .ThenInclude(od => od.Discount)
                .Include(od =>od.ShippingAddress)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return orders;
    }

    public async Task<List<Orders>> GetOrderHistoryByUserIdAsync(Guid userId)
    {
        var orders = await _context.Orders
            .Include(o => o.OrderDetails) // Không cần kiểm tra null
                .ThenInclude(od => od.Product)
            .Include(o => o.OrderDiscounts)
                .ThenInclude(od => od.Discount)
                   .Include(od => od.ShippingAddress)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return orders;
    }


}
