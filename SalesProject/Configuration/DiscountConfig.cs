using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class DiscountConfig : IEntityTypeConfiguration<Discounts>
    {
        public void Configure(EntityTypeBuilder<Discounts> builder)
        {
            builder.ToTable("Discounts");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Code)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(d => d.DiscountAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(d => d.DiscountType)
                   .IsRequired()
                   .HasDefaultValue(DiscountTypeEnum.FixedAmount)
                   .HasConversion<int>();

            builder.Property(d => d.ExpiryDate)
                   .IsRequired();

            builder.Property(d => d.MaxUsage)
                   .IsRequired()
                   .HasDefaultValue(1);

            builder.Property(d => d.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);
        }
    }
}
