using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class BlogPostConfig : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.ToTable("BlogPosts");

            builder.HasKey(bp => bp.Id);

            builder.Property(bp => bp.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(bp => bp.Content)
                   .IsRequired();

            builder.Property(bp => bp.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            builder.HasOne(bp => bp.BlogSubCategory)
                   .WithMany(sc => sc.BlogPosts)
                   .HasForeignKey(bp => bp.BlogSubCategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
