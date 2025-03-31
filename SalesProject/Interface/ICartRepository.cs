using SalesProject.Models.Domain;
using SalesProject.Models.DTOs.Request;

namespace SalesProject.Interface
{
    public interface ICartRepository
    {


        Task<Cart?> UpdateCartForUser(Cart Cart);

        Task<Cart> CreateCartforUser(Cart Cart);


        Task<Cart?> GetCartById(Guid CartId);



        Task<Cart?> DeleteProductFromCart(Guid userId, Guid productId);


        Task<int> ClearCartByUserId(Guid userId);



        Task<List<Cart>> GetCartsByUserId(Guid userId);


        Task<Cart> CreateOrUpdateCart(Cart cart);
    }
}
