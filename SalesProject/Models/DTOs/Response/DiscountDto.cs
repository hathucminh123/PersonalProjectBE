namespace SalesProject.Models.DTOs.Response
{
    public class DiscountDto
    {
        public Guid DiscountId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string DiscountType { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
    }
}
