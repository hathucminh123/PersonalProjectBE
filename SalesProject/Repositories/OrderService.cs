﻿using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

using System;
namespace SalesProject.Repositories;

public class OrderService : IOrdersRepository
{
    private readonly SalesDbContext _context;
    private readonly IEmailRepository _emailService;

    public OrderService(SalesDbContext context,IEmailRepository emailRepository)
    {
        _context = context;
        _emailService = emailRepository;
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

    //public async Task<Orders> CreateOrderAsync(
    //Guid userId,
    //Address shippingAddressInput,
    //PaymentMethodEnum paymentMethod,
    //string? discountCode = null)
    //{
    //    // Bước 1: Lưu địa chỉ mới
    //    var shippingAddress = new Address
    //    {
    //        Id = Guid.NewGuid(),
    //        UserId = userId,
    //        FullName = shippingAddressInput.FullName,
    //        Phone = shippingAddressInput.Phone,
    //        Province = shippingAddressInput.Province,
    //        District = shippingAddressInput.District,
    //        Ward = shippingAddressInput.Ward,
    //        StreetAddress = shippingAddressInput.StreetAddress,
    //        Email = shippingAddressInput.Email,
    //        IsDefault = false
    //    };
    //    _context.Addresses.Add(shippingAddress);
    //    await _context.SaveChangesAsync();

    //    // Bước 2: Lấy giỏ hàng
    //    var cartItems = await _context.Carts
    //        .Where(c => c.UserId == userId && (c.IsActive ?? false))
    //        .Include(c => c.Product)
    //        .ToListAsync();

    //    if (!cartItems.Any())
    //        throw new Exception("Giỏ hàng trống!");

    //    decimal totalPrice = cartItems.Sum(c => (c.Product?.FinalPrice ?? 0) * c.Quantity);
    //    decimal discountAmount = 0;

    //    // Bước 3: Áp dụng mã giảm giá
    //    Discounts?discount = null;
    //    if (!string.IsNullOrEmpty(discountCode))
    //    {
    //        discount = await _context.Discounts
    //            .FirstOrDefaultAsync(d => d.Code == discountCode && d.IsActive && d.ExpiryDate >= DateTime.UtcNow);

    //        if (discount != null)
    //        {
    //            discountAmount = discount.DiscountType == DiscountTypeEnum.Percentage
    //                ? totalPrice * (discount.DiscountAmount / 100)
    //                : discount.DiscountAmount;

    //            totalPrice -= discountAmount;
    //        }
    //    }

    //    // Bước 4: Tạo đơn hàng
    //    var order = new Orders
    //    {
    //        Id = Guid.NewGuid(),
    //        UserId = userId,
    //        ShippingAddressId = shippingAddress.Id,
    //        TotalPrice = totalPrice,
    //        DiscountAmount = discountAmount,
    //        PaymentMethod = paymentMethod,
    //        Status = OrderStatusEnum.Pending,
    //        CreatedAt = DateTime.UtcNow
    //    };

    //    _context.Orders.Add(order);
    //    await _context.SaveChangesAsync();

    //    // Bước 5: Tạo chi tiết đơn hàng
    //    foreach (var cartItem in cartItems)
    //    {
    //        var detail = new OrderDetails
    //        {
    //            OrderId = order.Id,
    //            ProductId = cartItem.ProductId,
    //            Quantity = cartItem.Quantity,
    //            UnitPrice = cartItem.Product?.FinalPrice ?? 0
    //        };
    //        _context.OrderDetails.Add(detail);

    //        // Giảm tồn kho nếu cần
    //        cartItem.Product.Stock -= cartItem.Quantity;
    //    }

    //    // Bước 6: Gắn mã giảm giá
    //    if (discount != null)
    //    {
    //        _context.OrderDiscounts.Add(new OrderDiscounts
    //        {
    //            OrderId = order.Id,
    //            DiscountId = discount.Id
    //        });
    //    }

    //    // Bước 7: Xoá giỏ hàng
    //    _context.Carts.RemoveRange(cartItems);

    //    await _context.SaveChangesAsync();

    //    return order;
    //}

    public async Task<Orders> CreateOrderAsync(
    Guid userId,
    Address? shippingAddressInput,
    Guid? shippingAddressId,
    PaymentMethodEnum paymentMethod,
    string? discountCode = null
        )
    {
        Address shippingAddress;

        // 🔹 Trường hợp dùng địa chỉ cũ (được truyền vào bằng ID)
        if (shippingAddressId.HasValue)
        {
            shippingAddress = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == shippingAddressId.Value && a.UserId == userId)
                ?? throw new Exception("Không tìm thấy địa chỉ đã lưu.");
        }
        else
        {
            // 🔹 Trường hợp tạo mới địa chỉ từ thông tin người dùng nhập
            if (shippingAddressInput == null )
                throw new Exception("Thông tin địa chỉ không hợp lệ.");

            shippingAddress = new Address
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
        }

        // 🔹 Lấy giỏ hàng
        var cartItems = await _context.Carts
            .Where(c => c.UserId == userId && (c.IsActive ?? false))
            .Include(c => c.Product)
            .ToListAsync();

        if (!cartItems.Any())
            throw new Exception("Giỏ hàng trống!");

        decimal totalPrice = cartItems.Sum(c => (c.Product?.FinalPrice ?? 0) * c.Quantity);
        decimal discountAmount = 0;
        Discounts? discount = null;

        // 🔹 Áp dụng mã giảm giá
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

        // 🔹 Tạo mã đơn hàng
        var orderCode = $"OD-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1000, 9999)}";

        // 🔹 Tạo đơn hàng
        var order = new Orders
        {
            Id = Guid.NewGuid(),
            OrderCode = orderCode,
            UserId = userId,
            ShippingAddressId = shippingAddress.Id,
            TotalPrice = totalPrice,
            DiscountAmount = discountAmount,
            PaymentMethod = paymentMethod,
            Status = OrderStatusEnum.Pending,
            PaymentStatus=PaymentStatusEnum.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // 🔹 Lưu chi tiết đơn hàng và cập nhật tồn kho
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

            cartItem.Product.Stock -= cartItem.Quantity;
        }

        // 🔹 Gắn mã giảm giá nếu có
        if (discount != null)
        {
            _context.OrderDiscounts.Add(new OrderDiscounts
            {
                OrderId = order.Id,
                DiscountId = discount.Id
            });
        }

        // 🔹 Xóa giỏ hàng
        _context.Carts.RemoveRange(cartItems);

        await _context.SaveChangesAsync();

        // 🔹 Gửi email xác nhận đơn hàng
        if (!string.IsNullOrEmpty(shippingAddress.Email))
        {
            var subject = "Xác nhận đơn hàng";
            var body = $"Xin chào {shippingAddress.FullName},\n\n" +
                       $"Cảm ơn bạn đã đặt hàng tại cửa hàng của chúng tôi.\n" +
                       $"Mã đơn hàng của bạn là: {order.OrderCode}\n\n" +
                       $"Chúng tôi sẽ xử lý đơn hàng trong thời gian sớm nhất.\n\n" +
                       $"Trân trọng.";

            await _emailService.SendEmailAsync(shippingAddress.Email, subject, body);
        }

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
