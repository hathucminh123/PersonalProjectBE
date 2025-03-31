using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class OrderDetailsConfig : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.ToTable("OrderDetails");

            builder.HasKey(od => od.Id); // Khóa chính

            // Thiết lập quan hệ với Orders
            builder.HasOne(od => od.Order)
                   .WithMany(o => o.OrderDetails) // Một Order có nhiều OrderDetails
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.Cascade); // Xóa Order thì xóa luôn OrderDetails

            // Thiết lập quan hệ với Products
            builder.HasOne(od => od.Product)
                   .WithMany() // Một Product có thể có nhiều OrderDetails
                   .HasForeignKey(od => od.ProductId)
                   .OnDelete(DeleteBehavior.Restrict); // Không xóa OrderDetails khi Product bị xóa

            // Định dạng số tiền
            builder.Property(od => od.UnitPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(od => od.Discount)
                   .HasColumnType("decimal(18,2)")
                   .HasDefaultValue(0);
        }
    }
}
