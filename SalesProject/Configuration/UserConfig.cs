using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesProject.Models.Domain;
using System;

namespace SalesProject.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<Users>
    {
        //public void Configure(EntityTypeBuilder<Users> builder)
        //{
        //    builder.ToTable("Users");

        //    builder.HasKey(u => u.Id);

        //    builder.HasIndex(u => u.Email).IsUnique();

        //    builder.Property(u => u.FullName)
        //        .IsRequired()
        //        .HasMaxLength(100);

        //    builder.Property(u => u.Email)
        //        .IsRequired()
        //        .HasMaxLength(100);

        //    builder.Property(u => u.Role)
        //        .IsRequired()
        //        .HasDefaultValue(UserRole.Customer)
        //        .HasConversion<int>(); // Lưu Enum dưới dạng số (1: Customer, 2: Admin)

        //    builder.Property(u => u.CreatedAt)
        //        .HasDefaultValueSql("GETUTCDATE()");

        //    // Tạo mật khẩu đã hash
        //    var user1Password = CreatePasswordHash("User1");
        //    var user2Password = CreatePasswordHash("User2");
        //    var customerPassword = CreatePasswordHash("Customer");
        //    var adminPassword = CreatePasswordHash("Admin");

        //    builder.HasData(
        //        new Users
        //        {
        //            Id = Guid.Parse("0f5f2ffb-9852-4195-8f4d-be03a4f949bc"), // Sử dụng Guid thay vì số nguyên
        //            FullName = "Customer",
        //            Email = "customer@example.com",
        //            PasswordHash = user1Password,
        //            Role = UserRole.Customer,
        //            CreatedAt = DateTime.UtcNow
        //        },
        //        new Users
        //        {
        //            Id = Guid.Parse("6c083302-a160-448a-b912-c9cefb3fef3e"),
        //            FullName = "Admin",
        //            Email = "admin@example.com",
        //            PasswordHash = adminPassword,
        //            Role = UserRole.Admin,
        //            CreatedAt = DateTime.UtcNow
        //        }
        //    );
        //}
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.EmailConfirmationCode)
             .HasMaxLength(256)
             .IsRequired(false);


            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            //builder.Property(u => u.Role)
            //    .HasConversion<int>()
            //    .HasDefaultValue(UserRole.Customer);

            builder.Property(u => u.Role)
              .HasConversion<string>()
              .HasDefaultValue(UserRole.Customer);

            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"); // <-- Chuẩn PostgreSQL

            builder.Property(u => u.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Seed Data cố định
            builder.HasData(
                new Users
                {
                    Id = Guid.Parse("0f5f2ffb-9852-4195-8f4d-be03a4f949bc"),
                    FullName = "Customer",
                    Email = "customer@example.com",
                    PasswordHash = CreatePasswordHash("Customer"),
                    Role = UserRole.Customer,
                    CreatedAt = new DateTime(2024, 3, 13, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 3, 13, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                },
                new Users
                {
                    Id = Guid.Parse("6c083302-a160-448a-b912-c9cefb3fef3e"),
                    FullName = "Admin",
                    Email = "admin@example.com",
                    PasswordHash = CreatePasswordHash("Admin"),
                    Role = UserRole.Admin,
                    CreatedAt = new DateTime(2024, 3, 13, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2024, 3, 13, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                }
            );
        }




        private string CreatePasswordHash(string password)
        {
            // Đây là cách đơn giản để hash mật khẩu (sử dụng SHA256)
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
