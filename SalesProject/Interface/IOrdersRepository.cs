using SalesProject.Models.Domain;

namespace SalesProject.Interface
{
    public interface IOrdersRepository
    {

        Task<Orders> CreateOrderAsync(Guid userId  , Address shippingAddressInput, Guid? shippingAddressId, PaymentMethodEnum paymentMethod,  string? discountCode = null);


        Task<List<Orders>> GetOrderHistoryByUserIdAsync(Guid userId);


        Task<List<Orders>> GetOrdersHistory();






    }
}
