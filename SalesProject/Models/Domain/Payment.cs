using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesProject.Models.Domain
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Orders? Order { get; set; } // Một Payment thuộc về một Order

        [Required]
        public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.CreditCard;

        [Required]
        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.Pending;

        [MaxLength(100)]
        public string? TransactionId { get; set; } // Giới hạn độ dài để tránh lưu trữ thừa
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // Số tiền thanh toán

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum PaymentStatusEnum
    {
        Pending = 0,   // Chờ xử lý
        Processing = 1, // Đang xử lý
        Completed = 2, // Đã thanh toán
        Failed = 3,    // Thanh toán thất bại
        Refunded = 4   // Đã hoàn tiền
    }

    public enum PaymentMethodEnum
    {
        CreditCard = 0,
        PayPal = 1,
        VNPay = 2,
        MoMo = 3,
        BankTransfer = 4
    }
}
