using SalesProject.Models.Domain;
using System;

namespace SalesProject.DTOs
{
    public class DiscountResponseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public DiscountTypeEnum DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int MaxUsage { get; set; }
        public bool IsActive { get; set; }
    }
}
