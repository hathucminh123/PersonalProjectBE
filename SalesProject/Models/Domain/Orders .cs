using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SalesProject.Models.Domain
{
    public class Orders
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }


        [ForeignKey("UserId")]
        public Users? User { get; set; } // Một Order thuộc về một User

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0; // Tổng giảm giá từ các mã giảm giá

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; } = 0;

        [Required]
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid ShippingAddressId { get; set; }
        public Address? ShippingAddress { get; set; }

        public PaymentMethodEnum PaymentMethod { get; set; }

        public string OrderCode { get; set; } = string.Empty;


        public ICollection<OrderDetails>? OrderDetails { get; set; }

        // 🔹 Quan hệ Nhiều-Nhiều: Một Order có thể có nhiều Discounts
        public ICollection<OrderDiscounts>? OrderDiscounts { get; set; }
    }

    public enum OrderStatusEnum
    {
        Pending = 0,     // Chờ xử lý
        Processing = 1,  // Đang xử lý
        Shipped = 2,     // Đã giao hàng
        Completed = 3,   // Hoàn thành
        Cancelled = 4,   // Đã hủy
        Refunded = 5     // Đã hoàn tiền
    }
}
