using Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesProject.Configuration;
using SalesProject.Models.Domain;
using System.Reflection.Emit;

namespace SalesProject.Data
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discounts> Discounts { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<Orders> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<OrderDiscounts> OrderDiscounts { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<CompareProduct> CompareProducts { get; set; }

        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new ShippingConfig());
            builder.ApplyConfiguration(new ReviewConfig());
            builder.ApplyConfiguration(new ProductConfig());
            builder.ApplyConfiguration(new PaymentConfig());
            builder.ApplyConfiguration(new OrderConfig());
            builder.ApplyConfiguration(new OrderDetailsConfig());
            builder.ApplyConfiguration(new CategoryConfig());
            builder.ApplyConfiguration(new CartConfig());
            builder.ApplyConfiguration(new OrderDiscountConfig());
            builder.ApplyConfiguration(new SubCategoryConfig());
            builder.ApplyConfiguration(new CompareProductConfig());
            builder.ApplyConfiguration(new AddressConfig());
            base.OnModelCreating(builder);

        }
    }
}
