using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class ProductConfig : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.ToTable("Products");

            // 🔹 Khóa chính
            builder.HasKey(p => p.Id);

            // 🔹 Tên sản phẩm (bắt buộc, tối đa 255 ký tự)
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            // 🔹 Mô tả sản phẩm (tối đa 1000 ký tự)
            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            // 🔹 Giá gốc (bắt buộc)
            builder.Property(p => p.OriginalPrice)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // 🔹 Số tiền được giảm (có thể null)
            builder.Property(p => p.DiscountAmount)
                   .HasColumnType("decimal(18,2)");

            // 🔹 Số lượng tồn kho (bắt buộc)
            builder.Property(p => p.Stock)
                   .IsRequired();

            // 🔹 URL hình ảnh sản phẩm (tối đa 500 ký tự)
            builder.Property(p => p.ImageUrl)
                   .HasMaxLength(500);

            // 🔹 Hiệu ứng da đặc biệt (tối đa 100 ký tự)
            builder.Property(p => p.SkinEffect)
                   .HasMaxLength(100);

            // 🔹 Ngày tạo (mặc định là ngày hiện tại)
            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // 🔹 Liên kết với SubCategory (1 - N)
            builder.HasOne(p => p.SubCategory)
                   .WithMany(sc => sc.Products)
                   .HasForeignKey(p => p.SubCategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ✅ Thêm thuộc tính mới

            // 🔥 Hoạt Chất Chính (Tối đa 1000 ký tự)
            builder.Property(p => p.ActiveIngredients)
                   .HasMaxLength(1000);

            // 🔥 EXP - HSD Chưa Mở Nắp (Số tháng)
            builder.Property(p => p.ExpShelfLife)
                   .IsRequired();

            // 🔥 PAO - HSD Đã Mở Nắp (Số tháng)
            builder.Property(p => p.PaoShelfLife)
                   .IsRequired();

            // 🔥 Loại Da (Tối đa 255 ký tự)
            builder.Property(p => p.SkinType)
                   .HasMaxLength(255);

            // 🔥 Công Dụng (Tối đa 1000 ký tự)
            builder.Property(p => p.Benefits)
                   .HasMaxLength(1000);

            // 🔥 Thành Phần Chi Tiết (Tối đa 3000 ký tự)
            builder.Property(p => p.Ingredients)
                   .HasMaxLength(3000);

            // 🔥 Công Dụng Chính (Tóm tắt nhanh, tối đa 1000 ký tự)
            builder.Property(p => p.MainBenefits)
                   .HasMaxLength(1000);

            // 🔥 Vấn Đề Về Da (Tối đa 1000 ký tự)
            builder.Property(p => p.SkinConcerns)
                   .HasMaxLength(1000);

            // 🔥 Đánh dấu sản phẩm BestSeller
            builder.Property(p => p.IsBestSeller)
                   .HasDefaultValue(false);

            // 🔥 Bỏ qua các thuộc tính được tính toán, không ánh xạ vào database
            builder.Ignore(p => p.FinalPrice);
            builder.Ignore(p => p.DiscountPercentage);
        }
    }
}
