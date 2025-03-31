using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SalesProject.Models.Domain;

public class CartConfig : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);

        // Quan hệ với Users
        builder.HasOne(c => c.User)
               .WithMany(u => u.CartItems)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        // Quan hệ với Products
        builder.HasOne(c => c.Product)
               .WithMany()
               .HasForeignKey(c => c.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.Quantity)
               .IsRequired()
               .HasDefaultValue(1);

        builder.Property(c => c.IsActive)
               .IsRequired()
               .HasDefaultValue(true);

        builder.Property(c => c.CreatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        builder.Property(c => c.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        // ✅ CHỐT: Ngăn người dùng tạo trùng sản phẩm trong giỏ hàng
        builder.HasIndex(c => new { c.UserId, c.ProductId }).IsUnique();
    }
}
