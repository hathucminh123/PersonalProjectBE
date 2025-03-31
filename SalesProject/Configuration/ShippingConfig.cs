using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;


namespace SalesProject.Configuration
{
    public class ShippingConfig : IEntityTypeConfiguration<Shipping>
    {
        public void Configure(EntityTypeBuilder<Shipping> builder)
        {
            builder.ToTable("Shippings");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Status)
                .IsRequired()
                .HasDefaultValue(ShippingStatus.Pending)
                .HasConversion<int>(); // Lưu Enum dưới dạng số nguyên

            builder.Property(s => s.ShippingAddress)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
