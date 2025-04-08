using SalesProject.Models.Domain;

namespace SalesProject.Interface
{
    public interface IBlogSubCategoryService
    {
        Task<IEnumerable<BlogSubCategory>> GetAllAsync();
        Task<BlogSubCategory?> GetByIdAsync(int id);
        Task<BlogSubCategory> CreateAsync(BlogSubCategory subCategory);
        Task<BlogSubCategory?> UpdateAsync(int id, BlogSubCategory subCategory);
        Task<bool> DeleteAsync(int id);

        Task<BlogSubCategory?> GetByNameAsync(string name);

    }
}
