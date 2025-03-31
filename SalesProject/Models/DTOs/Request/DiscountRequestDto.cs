using SalesProject.Models.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace SalesProject.DTOs
{
    public class DiscountRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        public DiscountTypeEnum DiscountType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Discount amount must be greater than 0")]
        public decimal DiscountAmount { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "MaxUsage must be greater than 0")]
        public int MaxUsage { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
