using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            // 🔹 Khóa chính
            builder.HasKey(p => p.Id);

            // 🔹 Thiết lập quan hệ với Orders
            builder.HasOne(p => p.Order)
                   .WithMany()
                   .HasForeignKey(p => p.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ✅ Thêm cấu hình cho `Amount`
            builder.Property(p => p.Amount)
                   .HasColumnType("decimal(18,2)") // Định dạng tiền tệ
                   .IsRequired();

            // 🔹 Enum cho PaymentMethod (lưu dưới dạng int)
            builder.Property(p => p.PaymentMethod)
                   .HasConversion<int>() // Lưu dưới dạng số nguyên
                   .IsRequired();

            // 🔹 Enum cho PaymentStatus (lưu dưới dạng int)
            builder.Property(p => p.PaymentStatus)
                   .HasConversion<int>()
                   .IsRequired();

            // 🔹 Giới hạn TransactionId và đặt là duy nhất
            builder.Property(p => p.TransactionId)
                   .HasMaxLength(100);

            builder.HasIndex(p => p.TransactionId)
                   .IsUnique();

            // 🔹 Dùng CURRENT_TIMESTAMP từ PostgreSQL cho CreatedAt
            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // 🚀 Có thể mở rộng thêm các thuộc tính khác nếu cần trong tương lai!
        }
    }
}
