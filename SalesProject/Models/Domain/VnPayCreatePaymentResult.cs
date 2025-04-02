namespace SalesProject.Models.Domain
{
    public class VnPayCreatePaymentResult
    {
        public Guid PaymentId { get; set; }
        public string PaymentUrl { get; set; } = string.Empty;
    }
}
