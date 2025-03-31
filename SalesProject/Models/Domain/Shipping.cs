using System;
using System.ComponentModel.DataAnnotations;

namespace SalesProject.Models.Domain
{
    public class Shipping
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrderId { get; set; }

        [MaxLength(50)]
        public string? ShippingMethod { get; set; }

        [Required, MaxLength(255)]
        public string? ShippingAddress { get; set; }

        public string? TrackingNumber { get; set; }
        public DateTime? EstimatedDelivery { get; set; }

        [MaxLength(20)]
        public ShippingStatus Status { get; set; } = ShippingStatus.Pending ;
    }


    public enum ShippingStatus
    {
        Pending = 0,       // Chờ xử lý
        Processing = 1,    // Đang xử lý
        Shipped = 2,       // Đã gửi
        Delivered = 3,     // Đã giao
        Cancelled = 4,     // Đã hủy
        Returned = 5       // Trả hàng
    }
}
