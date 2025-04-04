using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Data;
using SalesProject.Models.Domain;
using System.Reflection.Emit;

namespace SalesProject.Configuration
{
    public class FavoriteProductConfig : IEntityTypeConfiguration<FavoriteProducts>
    {
        public void Configure(EntityTypeBuilder<FavoriteProducts> builder)
        {
            builder.ToTable("FavoriteProducts");

            builder.HasKey(fp => new { fp.UserId, fp.ProductId });



            builder.HasOne(builder => builder.User)
                .WithMany(user => user.favoriteProducts)
                .HasForeignKey(builder => builder.UserId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.HasOne(builder => builder.Product)
                .WithMany(product => product.favoriteProducts)
                .HasForeignKey(builder => builder.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

        }

       
    }
 
}
