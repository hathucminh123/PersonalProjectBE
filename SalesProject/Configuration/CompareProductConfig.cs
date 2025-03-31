using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class CompareProductConfig : IEntityTypeConfiguration<CompareProduct>
    {
        public void Configure(EntityTypeBuilder<CompareProduct> builder)
        {
            builder.ToTable("CompareProducts");

            builder.HasKey(cp => cp.Id);

            builder.Property(cp => cp.UserId)
                   .IsRequired();

            builder.Property(cp => cp.ProductId)
                   .IsRequired();

            builder.Property(cp => cp.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
