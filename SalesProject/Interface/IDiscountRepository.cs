using SalesProject.DTOs;
using SalesProject.Models.Domain;

namespace SalesProject.Interface
{
    public interface IDiscountRepository
    {

        Task<Discounts> CreateDiscount(Discounts Discounts);


        Task<List<Discounts>> GetDiscounts();


        Task<Discounts> DeleteDiscount (Guid id);   
    }
}
