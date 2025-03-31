using SalesProject.Models.Domain;

namespace SalesProject.Interface
{
    public interface ICategory
    {

        Task<List<Category>> GetAsync();

        Task<Category> GetCategorybyIdAsync(Guid id);


        Task<Category> CreateCategoryAsync(Category category);


        Task<Category> UpdateCategoryAsync(Guid id ,Category category);


        Task<Category> DeleteCategoryAsync(Guid id);
    }
}
