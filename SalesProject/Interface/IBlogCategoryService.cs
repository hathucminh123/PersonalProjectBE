using SalesProject.Models.Domain;

namespace SalesProject.Interface
{
    public interface IBlogCategoryService
    {
        Task<IEnumerable<BlogCategory>> GetAllAsync();
        Task<BlogCategory?> GetByIdAsync(int id);
        Task<BlogCategory> CreateAsync(BlogCategory category);
        Task<BlogCategory?> UpdateAsync(int id, BlogCategory category);
        Task<bool> DeleteAsync(int id);
    }
}
