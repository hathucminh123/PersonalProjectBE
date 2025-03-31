using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

namespace SalesProject.Repositories
{
    public class CartService : ICartRepository
    {
        private readonly SalesDbContext salesDbContext;

        public CartService(SalesDbContext salesDbContext ) {
            this.salesDbContext = salesDbContext;
        }
        public async Task<Cart> CreateCartforUser(Cart Cart)
        {
            await salesDbContext.Carts.AddAsync(Cart);
            await salesDbContext.SaveChangesAsync();
            return    Cart;
        }


        

        public async Task<Cart?> DeleteProductFromCart(Guid userId, Guid productId)
        {
            var cartItem = await salesDbContext.Carts
                .Where(c => c.UserId == userId && c.ProductId == productId)
                .FirstOrDefaultAsync();

            if (cartItem != null)
            {
                salesDbContext.Carts.Remove(cartItem);
                await salesDbContext.SaveChangesAsync();
                return cartItem;
            }

            return null;
        }


        public async Task<int> ClearCartByUserId(Guid userId)
        {
            var cartItems = await salesDbContext.Carts
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (cartItems.Any())
            {
                salesDbContext.Carts.RemoveRange(cartItems);
                await salesDbContext.SaveChangesAsync();
            }

            return cartItems.Count;
        }



        public async Task<Cart?> GetCartById(Guid CartId)
        {
            return await salesDbContext.Carts.Include(c => c.User).Include(c => c.Product).FirstOrDefaultAsync(x => x.Id == CartId);
        }

        //public async Task<List<Cart>> GetCartsByUserId(Guid userId)
        //{
        //    var carts = await salesDbContext.Carts
        //        .Include(c => c.Product) // Include product để lấy đầy đủ thông tin
        //        .Where(c => c.UserId == userId && (c.IsActive ?? false))
        //        .ToListAsync();

        //    var groupedCarts = carts
        //        .GroupBy(c => c.ProductId)
        //        .Select(group => new Cart
        //        {
        //            Id = group.First().Id,  // hoặc Guid.NewGuid() nếu bạn muốn tạo ID mới
        //            UserId = userId,
        //            ProductId = group.Key,
        //            Product = group.First().Product, // lấy thông tin sản phẩm đầu tiên (do trùng nhau)
        //            Quantity = group.Sum(c => c.Quantity), // tổng hợp quantity
        //            CreatedAt = group.Min(c => c.CreatedAt), // thời gian đầu tiên thêm vào giỏ
        //            UpdatedAt = group.Max(c => c.UpdatedAt), // thời gian cập nhật gần nhất
        //            IsActive = true,
        //        })
        //        .ToList();

        //    return groupedCarts;
        //}
        public async Task<List<Cart>> GetCartsByUserId(Guid userId)
        {
            return await salesDbContext.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId && (c.IsActive ?? false))
                .ToListAsync();
        }

        public async Task<Cart?> UpdateCartForUser(Cart cart)
        {
            if (cart.Quantity < 1)
            {
                // Có thể throw exception hoặc return null tùy cách bạn xử lý phía controller/service
                return null;
            }

            var existingCartItem = await salesDbContext.Carts
                .FirstOrDefaultAsync(c => c.UserId == cart.UserId && c.ProductId == cart.ProductId && (c.IsActive ?? true));

            if (existingCartItem == null)
            {
                return null;
            }

            existingCartItem.Quantity = cart.Quantity;
            existingCartItem.UpdatedAt = DateTime.UtcNow;

            salesDbContext.Carts.Update(existingCartItem);
            await salesDbContext.SaveChangesAsync();

            return existingCartItem;
        }

        public async Task<Cart> CreateOrUpdateCart(Cart cart)
        {
            var existing = await salesDbContext.Carts
                .FirstOrDefaultAsync(c => c.UserId == cart.UserId && c.ProductId == cart.ProductId && (c.IsActive ?? true));

            if (existing != null)
            {
                existing.Quantity += cart.Quantity;
                existing.UpdatedAt = DateTime.UtcNow;
                salesDbContext.Carts.Update(existing);
                await salesDbContext.SaveChangesAsync();
                return existing;
            }
            else
            {
                cart.CreatedAt = DateTime.UtcNow;
                cart.UpdatedAt = DateTime.UtcNow;
                await salesDbContext.Carts.AddAsync(cart);
                await salesDbContext.SaveChangesAsync();
                return cart;
            }
        }


    }
}
