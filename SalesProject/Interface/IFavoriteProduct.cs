using SalesProject.Models.Domain;

namespace SalesProject.Interface
{
    public interface IFavoriteProduct
    {

        Task<FavoriteProducts> AddFavoriteProductAsync(Guid productId, Guid userId);
        Task<List<Products>> GetAllFavoriteProductsAsyncByUserId(Guid userId);
        Task<FavoriteProducts> RemoveFavoriteProductAsync(Guid productId, Guid userId);
    }
}
