using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class SubCategoryConfig : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.ToTable("SubCategories");

            // 🔹 Khóa chính
            builder.HasKey(sc => sc.Id);

            // 🔹 Tên danh mục phụ (Bắt buộc, tối đa 100 ký tự)
            builder.Property(sc => sc.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            // 🔹 Mô tả danh mục phụ (Tối đa 500 ký tự)
            builder.Property(sc => sc.Description)
                   .HasMaxLength(500);

            // 🔹 Trạng thái hoạt động
            builder.Property(sc => sc.IsActive)
                   .HasDefaultValue(true)
                   .IsRequired();

            // 🔹 Thời gian tạo
            builder.Property(sc => sc.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // 🔹 Khai báo khóa ngoại trỏ về Category
            builder.HasOne(sc => sc.Category)
                   .WithMany(c => c.SubCategories)
                   .HasForeignKey(sc => sc.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade); // Xóa Category sẽ xóa các SubCategory liên quan

            // 🔹 Không lưu trực tiếp ProductCount vào DB
            builder.Ignore(sc => sc.ProductCount);
        }
    }
}
