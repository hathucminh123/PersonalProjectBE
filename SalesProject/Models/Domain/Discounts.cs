using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SalesProject.Models.Domain
{
    public class Discounts
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(50)]
        public string Code { get; set; } = string.Empty; // Mã giảm giá (VD: "SUMMER2024")

        [Required]
        public DiscountTypeEnum DiscountType { get; set; } 

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } // Số tiền giảm hoặc phần trăm giảm

        [Required]
        public DateTime ExpiryDate { get; set; } // Ngày hết hạn

        [Required]
        public int MaxUsage { get; set; } = 1; // Số lần có thể sử dụng

        public bool IsActive { get; set; } = true; // Mã giảm giá có còn hiệu lực không?

        // 🔹 Quan hệ Nhiều-Nhiều: Một Discount có thể áp dụng cho nhiều Orders
        public ICollection<OrderDiscounts>? OrderDiscounts { get; set; }
    }

    public enum DiscountTypeEnum
    {
        FixedAmount = 0,  // Giảm số tiền cố định (USD)
        Percentage = 1     // Giảm theo phần trăm (%)
    }
}
