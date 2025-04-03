namespace SalesProject.Models.Domain
{
    public class PaymentInformationModel
    {
        public string? OrderType { get; set; } = "VnPay";
        //public double Amount { get; set; }
        public string? OrderDescription { get; set; }
        public string? Name { get; set; }


        public Guid OrderId { get; set; }

    }
}
