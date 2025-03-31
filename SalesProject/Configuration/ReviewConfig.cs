using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class ReviewConfig : IEntityTypeConfiguration<Reviews>
    {
        public void Configure(EntityTypeBuilder<Reviews> builder)
        {
            // Đặt tên bảng trong SQL
            builder.ToTable("Reviews");

            // Định nghĩa khóa chính
            builder.HasKey(r => r.Id);

            // Thiết lập quan hệ với bảng Users
            builder.HasOne(r => r.User)
                   .WithMany() // Một User có thể có nhiều Review
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Xóa User sẽ xóa luôn Review

            // Thiết lập quan hệ với bảng Products
            builder.HasOne(r => r.Product)
                   .WithMany() // Một Product có thể có nhiều Review
                   .HasForeignKey(r => r.ProductId)
                   .OnDelete(DeleteBehavior.Cascade); // Xóa Product sẽ xóa luôn Review

            // Cấu hình Rating (phải từ 1 đến 5)
            builder.Property(r => r.Rating)
                   .IsRequired()
                   .HasDefaultValue(1); // Giá trị mặc định là 1

            // Định nghĩa độ dài tối đa cho Comment
            builder.Property(r => r.Comment)
                   .HasMaxLength(1000);

            // Tạo cột CreatedAt với giá trị mặc định là thời gian hiện tại
            builder.Property(r => r.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
        }
    }
}
