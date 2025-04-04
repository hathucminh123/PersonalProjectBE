// File: Repositories/FavoriteProductService.cs
using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

namespace SalesProject.Repositories
{
    public class FavoriteProductService : IFavoriteProduct
    {
        private readonly SalesDbContext salesDbContext;

        public FavoriteProductService(SalesDbContext salesDbContext)
        {
            this.salesDbContext = salesDbContext;
        }

    
        public async Task<FavoriteProducts> AddFavoriteProductAsync(Guid productId, Guid userId)
        {
            var product = await salesDbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (product == null) throw new Exception("Product not found");

            var user = await salesDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) throw new Exception("User not found");

            // Kiểm tra nếu đã tồn tại thì không thêm nữa
            var existing = await salesDbContext.FavoriteProducts
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == userId);
            if (existing != null) throw new Exception("Product already favorited");

            var favoriteProduct = new FavoriteProducts
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                UserId = userId,
                Product = product,
                User = user
            };

            await salesDbContext.FavoriteProducts.AddAsync(favoriteProduct);
            await salesDbContext.SaveChangesAsync();

            return favoriteProduct;
        }

   

        public async Task<List<Products>> GetAllFavoriteProductsAsyncByUserId(Guid userId)
        {
            var user = await salesDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) throw new Exception("User not found");

            //var favoriteProducts = await salesDbContext.FavoriteProducts
            //    .Where(fp => fp.UserId == userId)
            //    .Include(fp => fp.Product)
            //    .ToListAsync();

            var productIds = await salesDbContext.FavoriteProducts
                .Where(fp => fp.UserId == userId)
                .Select(fp => fp.ProductId)
                .ToListAsync();

            return await salesDbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }

  

        public async Task<FavoriteProducts> RemoveFavoriteProductAsync(Guid productId, Guid userId)
        {
            var favoriteProduct = await salesDbContext.FavoriteProducts
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == userId);

            if (favoriteProduct == null)
            {
                throw new Exception("Favorite product not found");
            }

            salesDbContext.FavoriteProducts.Remove(favoriteProduct);
            await salesDbContext.SaveChangesAsync();

            return favoriteProduct;
        }
    }
}
