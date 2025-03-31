using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesProject.Repositories
{
    public class ProductService : IProductRepository
    {
        private readonly SalesDbContext _context;

        public ProductService(SalesDbContext salesDbContext)
        {
            _context = salesDbContext;
        }

        // Thêm một sản phẩm
        public async Task<Products> AddProduct(Products product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        // Xóa sản phẩm theo ID
        public async Task<Products?> DeleteProduct(Guid productId)
        {
            var productdomain = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (productdomain == null)
            {
                return null; // Sản phẩm không tìm thấy
            }
            _context.Products.Remove(productdomain);
            await _context.SaveChangesAsync();
            return productdomain; // Trả về sản phẩm đã xóa
        }

        // Lấy tất cả sản phẩm, bao gồm cả thông tin về SubCategory
        public async Task<List<Products>> GetAllProduct()
        {
            return await _context.Products
                .Include(p => p.SubCategory)  // Bao gồm thông tin SubCategory
                .ToListAsync();
        }

        // Lấy thông tin sản phẩm theo ID, bao gồm cả SubCategory
        public async Task<Products?> GetProductById(Guid productId)
        {
            var product = await _context.Products
                .Include(p => p.SubCategory)  // Bao gồm thông tin SubCategory
                .FirstOrDefaultAsync(p => p.Id == productId);

            return product; // Nếu không tìm thấy, trả về null
        }

        // Cập nhật thông tin sản phẩm
        public async Task<Products?> UpdateProduct(Guid productId, Products product)
        {
            // Tìm sản phẩm theo ID
            var productdomain = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (productdomain == null)
            {
                return null; // Sản phẩm không tìm thấy
            }

            // Cập nhật các thuộc tính của sản phẩm
            productdomain.Name = product.Name;
            productdomain.Description = product.Description;
            productdomain.OriginalPrice = product.OriginalPrice;
            productdomain.DiscountAmount = product.DiscountAmount;
            productdomain.Stock = product.Stock;
            productdomain.ImageUrl = product.ImageUrl;
            productdomain.ActiveIngredients = product.ActiveIngredients;
            productdomain.Benefits = product.Benefits;
            productdomain.SkinType = product.SkinType;
            productdomain.Ingredients = product.Ingredients;
            productdomain.ExpShelfLife = product.ExpShelfLife;
            productdomain.PaoShelfLife = product.PaoShelfLife;
            productdomain.IsBestSeller = product.IsBestSeller;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.Products.Update(productdomain);
            await _context.SaveChangesAsync();

            return productdomain; // Trả về sản phẩm đã cập nhật
        }
    }
}
