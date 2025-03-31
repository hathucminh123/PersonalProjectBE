using SalesProject.Models.Domain;

namespace SalesProject.Interface
{
    public interface IPaymentRespository
    {

        Task<Payment> ProcessPaymentAsync(Guid orderId, PaymentMethodEnum paymentMethodEnum);


    }
}
