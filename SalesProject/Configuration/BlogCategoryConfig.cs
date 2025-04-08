using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class BlogCategoryConfig : IEntityTypeConfiguration<BlogCategory>
    {
        public void Configure(EntityTypeBuilder<BlogCategory> builder)
        {
            builder.ToTable("BlogCategories");

            builder.HasKey(bc => bc.Id);

            builder.Property(bc => bc.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(bc => bc.Slug)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasMany(bc => bc.SubCategories)
                   .WithOne(sc => sc.BlogCategory)
                   .HasForeignKey(sc => sc.BlogCategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
