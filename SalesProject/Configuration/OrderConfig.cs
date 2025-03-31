using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;
using System.Reflection.Emit;

namespace SalesProject.Configuration
{
    public class OrderConfig : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.ToTable("Orders"); // Đặt tên bảng

            builder.HasKey(o => o.Id); // Định nghĩa khóa chính

            // 🔹 Quan hệ với Users
            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders) // Một User có nhiều Orders
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Xóa User thì xóa luôn Order

            // 🔹 Quan hệ với OrderDetails (1-Nhiều)
            builder.HasMany(o => o.OrderDetails)
                   .WithOne(od => od.Order)
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.Cascade); // Xóa Order thì xóa OrderDetails

            // 🔹 Quan hệ với OrderDiscounts (1-Nhiều)
            builder.HasMany(o => o.OrderDiscounts)
                   .WithOne(od => od.Order)
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.Cascade); // Xóa Order thì xóa OrderDiscounts


            builder.HasOne(o => o.ShippingAddress)
                  .WithMany()
                  .HasForeignKey(o => o.ShippingAddressId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Cấu hình kiểu dữ liệu decimal
            builder.Property(o => o.TotalPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(o => o.DiscountAmount)
                   .HasColumnType("decimal(18,2)")
                   .HasDefaultValue(0);

            builder.Property(o => o.ShippingFee)
                   .HasColumnType("decimal(18,2)")
                   .HasDefaultValue(0);

            // 🔹 Lưu Enum dưới dạng số (int)
            builder.Property(o => o.Status)
                   .HasDefaultValue(OrderStatusEnum.Pending)
            .HasConversion<int>();

            builder
    .Property(o => o.PaymentMethod)
    .HasConversion<string>();

            builder.Property(o => o.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
        }
    }
}
