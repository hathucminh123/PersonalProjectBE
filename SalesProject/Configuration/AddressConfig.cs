using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;

namespace SalesProject.Configuration
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(a => a.Id);

            // 🔹 Quan hệ với User
            builder.HasOne(a => a.User)
                   .WithMany(u => u.Addresses)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Thuộc tính bắt buộc
            builder.Property(a => a.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Phone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(a => a.Email)
                   .HasMaxLength(100);

            builder.Property(a => a.Province)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.District)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Ward)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.StreetAddress)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(a => a.IsDefault)
                   .HasDefaultValue(false);

            builder.Property(a => a.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
        }
    }
}
