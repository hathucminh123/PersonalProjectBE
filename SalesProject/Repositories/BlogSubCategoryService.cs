using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

namespace SalesProject.Repositories
{
    public class BlogSubCategoryService : IBlogSubCategoryService
    {
        private readonly SalesDbContext _context;

        public BlogSubCategoryService(SalesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogSubCategory>> GetAllAsync()
        {
            return await _context.BlogSubCategories
                .Include(sc => sc.BlogCategory)
                .ToListAsync();
        }

        public async Task<BlogSubCategory?> GetByIdAsync(int id)
        {
            return await _context.BlogSubCategories
                .Include(sc => sc.BlogCategory)
                .Include(sc => sc.BlogPosts) // ✅ Include BlogPosts
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<BlogSubCategory?> GetByNameAsync(string name)
        {
            return await _context.BlogSubCategories
                .Include(sc => sc.BlogCategory)
                .Include(sc => sc.BlogPosts) // ✅ Include BlogPosts
                .FirstOrDefaultAsync(sc => sc.Name == name);
        }

        public async Task<BlogSubCategory> CreateAsync(BlogSubCategory subCategory)
        {
            _context.BlogSubCategories.Add(subCategory);
            await _context.SaveChangesAsync();
            return subCategory;
        }

        public async Task<BlogSubCategory?> UpdateAsync(int id, BlogSubCategory subCategory)
        {
            var existing = await _context.BlogSubCategories.FindAsync(id);
            if (existing == null) return null;

            existing.Name = subCategory.Name;
            existing.Slug = subCategory.Slug;
            existing.BlogCategoryId = subCategory.BlogCategoryId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subCategory = await _context.BlogSubCategories.FindAsync(id);
            if (subCategory == null) return false;

            _context.BlogSubCategories.Remove(subCategory);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
