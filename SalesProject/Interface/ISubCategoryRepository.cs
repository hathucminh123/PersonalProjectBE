
 using SalesProject.Models.Domain;
namespace SalesProject.Interface;

    public interface ISubCategoryRepository
{
    Task<List<SubCategory>> GetAllAsync();


    Task<SubCategory?> GetSubCategorybyIdAsync(Guid id);


    Task<SubCategory> CreateSubCategoryAsync(SubCategory subCategory);
    Task<SubCategory?> UpdateSubCategoryAsync(Guid id, SubCategory subCategory);

    Task<SubCategory?> DeleteSubCategoryAsync(Guid id);


}



