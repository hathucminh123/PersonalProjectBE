using Google;
using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

namespace SalesProject.Repositories
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly SalesDbContext _context;

        public BlogCategoryService(SalesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogCategory>> GetAllAsync() =>
            await _context.BlogCategories.Include(c => c.SubCategories).ThenInclude(c => c.BlogPosts).ToListAsync();

        public async Task<BlogCategory?> GetByIdAsync(int id) =>
            await _context.BlogCategories.Include(c => c.SubCategories).ThenInclude(c => c.BlogPosts)
                                         .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<BlogCategory> CreateAsync(BlogCategory category)
        {
            _context.BlogCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<BlogCategory?> UpdateAsync(int id, BlogCategory category)
        {
            var existing = await _context.BlogCategories.FindAsync(id);
            if (existing == null) return null;

            existing.Name = category.Name;
            existing.Slug = category.Slug;
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.BlogCategories.FindAsync(id);
            if (category == null) return false;

            _context.BlogCategories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
