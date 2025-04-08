using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

namespace SalesProject.Repositories
{
    public class BlogPostService : IBlogPostService
    {
        private readonly SalesDbContext _context;

        public BlogPostService(SalesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _context.BlogPosts
                .Include(p => p.BlogSubCategory)
                .ThenInclude(sc => sc.BlogCategory)
                .ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(int id)
        {
            return await _context.BlogPosts
                .Include(p => p.BlogSubCategory)
                .ThenInclude(sc => sc.BlogCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<BlogPost> CreateAsync(BlogPost post)
        {
            post.CreatedAt = DateTime.UtcNow;
            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<BlogPost?> UpdateAsync(int id, BlogPost post)
        {
            var existing = await _context.BlogPosts.FindAsync(id);
            if (existing == null) return null;

            existing.Title = post.Title;
            existing.ImageUrl = post.ImageUrl;
            existing.Content = post.Content;
            existing.BlogSubCategoryId = post.BlogSubCategoryId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var post = await _context.BlogPosts.FindAsync(id);
            if (post == null) return false;

            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
