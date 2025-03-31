using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class OrderDiscountConfig : IEntityTypeConfiguration<OrderDiscounts>
    {
        public void Configure(EntityTypeBuilder<OrderDiscounts> builder)
        {
            builder.ToTable("OrderDiscounts");

            builder.HasKey(od => new { od.OrderId, od.DiscountId }); // Tạo khóa chính là cặp OrderId + DiscountId

            // Thiết lập quan hệ với Orders
            builder.HasOne(od => od.Order)
                   .WithMany(o => o.OrderDiscounts)
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Thiết lập quan hệ với Discounts
            builder.HasOne(od => od.Discount)
                   .WithMany(d => d.OrderDiscounts)
                   .HasForeignKey(od => od.DiscountId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
