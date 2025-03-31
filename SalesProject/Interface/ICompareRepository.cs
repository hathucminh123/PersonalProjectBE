using SalesProject.Models.Domain;

namespace SalesProject.Interface
{
    public interface ICompareRepository
    {
        Task<bool> AddToCompare(Guid userId, Guid productId);
        Task<bool> RemoveFromCompare(Guid userId, Guid productId);
        Task<List<Products>> GetCompareList(Guid userId);
    }
}
