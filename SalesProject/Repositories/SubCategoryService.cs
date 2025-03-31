using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

namespace SalesProject.Repositories
{
    public class SubCategoryService : ISubCategoryRepository
    {
        private readonly SalesDbContext salesDbContext;

        public SubCategoryService(SalesDbContext salesDbContext)
        {
            this.salesDbContext = salesDbContext;
        }
        public async Task<SubCategory> CreateSubCategoryAsync(SubCategory subCategory)
        {
            
            await salesDbContext.SubCategories.AddAsync(subCategory);
            await salesDbContext.SaveChangesAsync();

            return subCategory;

        }

        public async Task<SubCategory?> DeleteSubCategoryAsync(Guid id)
        {
            var subcategory = await salesDbContext.SubCategories.FirstOrDefaultAsync(x => x.Id ==id);
            if (subcategory == null)
            {
                return null;
            }
            salesDbContext.SubCategories.Remove(subcategory);
            await salesDbContext.SaveChangesAsync();
            return subcategory;
        }

        public async Task<List<SubCategory>> GetAllAsync()
        {
           return await salesDbContext.SubCategories.Include(x =>x.Category).ToListAsync();

        }

        public async  Task<SubCategory?> GetSubCategorybyIdAsync(Guid id)
        {
            var subcategory = await salesDbContext.SubCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (subcategory == null)
            {
                return null;
            }

            return subcategory;
        }

        public async Task<SubCategory?> UpdateSubCategoryAsync(Guid id, SubCategory subCategory)
        {
            var subcategoryDomain = await salesDbContext.SubCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (subcategoryDomain == null)
            {
                return null;
            }
            subcategoryDomain.Name = subCategory.Name;
            subcategoryDomain.Description = subCategory.Description;
            subcategoryDomain.IsActive = subCategory.IsActive;
            subcategoryDomain.CategoryId = subCategory.CategoryId;
            subcategoryDomain.CreatedAt = DateTime.UtcNow;

            await salesDbContext.SaveChangesAsync();

            return subcategoryDomain;


        }
    }
}
