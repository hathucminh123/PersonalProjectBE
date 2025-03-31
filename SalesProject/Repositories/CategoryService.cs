using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

namespace SalesProject.Repositories
{
    public class CategoryService : ICategory
    {
        private readonly SalesDbContext salesDbContext;

        public CategoryService(SalesDbContext salesDbContext)
        {
            this.salesDbContext = salesDbContext;
        }

        // Create Category
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await salesDbContext.Categories.AddAsync(category);
            await salesDbContext.SaveChangesAsync();
            return category;
        }

        // Delete Category
        public async Task<Category> DeleteCategoryAsync(Guid id)
        {
            var category = await salesDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return null;
            }
            salesDbContext.Categories.Remove(category);
            await salesDbContext.SaveChangesAsync();
            return category;
        }

        // Get All Categories
        public async Task<List<Category>> GetAsync()
        {
            return await salesDbContext.Categories
    .Include(c => c.SubCategories)
        .ThenInclude(sc => sc.Products)
    .ToListAsync();

        }

        // Get Category by ID
        public async Task<Category> GetCategorybyIdAsync(Guid id)
        {
            var category = await salesDbContext.Categories
                .Include(x => x.SubCategories)
                   .ThenInclude(sc => sc.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(category == null)
            {
                return null;
            }

            return category;
        }

        // Update Category
        public async Task<Category> UpdateCategoryAsync(Guid id, Category category)
        {
            var existingCategory = await salesDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            existingCategory.IsActive = category.IsActive;
            existingCategory.CreatedAt = DateTime.UtcNow;

            await salesDbContext.SaveChangesAsync();

            return existingCategory;
        }
    }
}
