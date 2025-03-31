using SalesProject.Models.Domain;

namespace SalesProject.Interface
{
    public interface IProductRepository
    {

        Task<List<Products>> GetAllProduct();

        Task<Products?> GetProductById(Guid productId);

        Task<Products> AddProduct(Products product);

        Task<Products?> UpdateProduct(Guid productId, Products product);

        Task<Products?> DeleteProduct(Guid productId);
    }
}
