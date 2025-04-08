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

            // Configure the primary key
            builder.HasKey(fp => new { fp.UserId, fp.ProductId });

            //builder.Property(cp => cp.UserId)
            //       .IsRequired();


            //builder.Property(cp => cp.ProductId)
            //       .IsRequired();


            builder.HasOne(cp => cp.User)
                   .WithMany(user => user.CompareProducts)
                   .HasForeignKey(cp => cp.UserId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(cp => cp.Products)
                     .WithMany(product => product.CompareProducts)
                     .HasForeignKey(cp => cp.ProductId)
                     .OnDelete(DeleteBehavior.Cascade);


            builder.Property(cp => cp.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
