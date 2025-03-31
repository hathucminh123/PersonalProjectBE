using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            // 🔹 Khóa chính
            builder.HasKey(c => c.Id);

            // 🔹 Tên danh mục (Bắt buộc, tối đa 100 ký tự)
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            // 🔹 Mô tả danh mục (Tối đa 500 ký tự)
            builder.Property(c => c.Description)
                   .HasMaxLength(500);

            // 🔹 Trạng thái danh mục (Mặc định là true)
            builder.Property(c => c.IsActive)
                   .HasDefaultValue(true)
                   .IsRequired();

            // 🔹 Thời gian tạo (Dùng lệnh SQL để tạo giá trị mặc định)
            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // 🔹 Quan hệ 1:N với SubCategory
            builder.HasMany(c => c.SubCategories)
                   .WithOne(sc => sc.Category)
                   .HasForeignKey(sc => sc.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade); // Xóa Category sẽ xóa các SubCategory liên quan

            // 🔹 Tự động tính toán ProductCount (Không lưu trực tiếp vào DB)
            builder.Ignore(c => c.ProductCount);
        }
    }
}
