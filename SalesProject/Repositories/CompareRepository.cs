using Google;
using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

namespace SalesProject.Repository
{
    public class CompareRepository : ICompareRepository
    {
        private readonly SalesDbContext _context;

        public CompareRepository(SalesDbContext context)
        {
            _context = context;
        }

        // ✅ Thêm sản phẩm vào danh sách so sánh
        public async Task<bool> AddToCompare(Guid userId, Guid productId)
        {
            var exists = await _context.CompareProducts
                .AnyAsync(cp => cp.UserId == userId && cp.ProductId == productId);

            if (!exists)
            {
                var compareProduct = new CompareProduct
                {
                    UserId = userId,
                    ProductId = productId
                };

                _context.CompareProducts.Add(compareProduct);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        // ✅ Xoá sản phẩm khỏi danh sách so sánh
        public async Task<bool> RemoveFromCompare(Guid userId, Guid productId)
        {
            var compareProduct = await _context.CompareProducts
                .FirstOrDefaultAsync(cp => cp.UserId == userId && cp.ProductId == productId);

            if (compareProduct != null)
            {
                _context.CompareProducts.Remove(compareProduct);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        // ✅ Lấy danh sách sản phẩm được thêm vào để so sánh
        public async Task<List<Products>> GetCompareList(Guid userId)
        {
            var productIds = await _context.CompareProducts
                .Where(cp => cp.UserId == userId)
                .Select(cp => cp.ProductId)
                .ToListAsync();

            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            return products;
        }
    }
}
