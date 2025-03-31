using System;

namespace SalesProject.Dtos
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } =string.Empty;
        public string? TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

