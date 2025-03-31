using SalesProject.Models.Domain;

namespace SalesProject.Models.DTOs.Request
{
    public class CreateOrderRequest
    {

        public Guid userId { get; set; }


        public CreateAddressRequest?shippingAddressInput { get; set; }

        public PaymentMethodEnum PaymentMethod{ get; set; }
        public string DiscountCode { get; set; } = string.Empty;



    }
}
