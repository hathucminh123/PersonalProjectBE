using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class BlogSubCategoryConfig : IEntityTypeConfiguration<BlogSubCategory>
    {
        public void Configure(EntityTypeBuilder<BlogSubCategory> builder)
        {
            builder.ToTable("BlogSubCategories");

            builder.HasKey(sc => sc.Id);

            builder.Property(sc => sc.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(sc => sc.Slug)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasOne(sc => sc.BlogCategory)
                   .WithMany(bc => bc.SubCategories)
                   .HasForeignKey(sc => sc.BlogCategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sc => sc.BlogPosts)
                   .WithOne(bp => bp.BlogSubCategory)
                   .HasForeignKey(bp => bp.BlogSubCategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
