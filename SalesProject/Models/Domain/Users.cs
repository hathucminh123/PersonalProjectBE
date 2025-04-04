using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesProject.Models.Domain
{
    public class Users 
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        public string? PasswordHash { get; set; } // Mật khẩu (chỉ bắt buộc nếu không có GoogleId)

        public string? GoogleId { get; set; } // ID Google nếu đăng nhập bằng Google

        public string? AvatarUrl { get; set; } // Ảnh đại diện

        [MaxLength(15)]
        public string? Phone { get; set; } // Số điện thoại

        [MaxLength(255)]
        public string? Address { get; set; } // Địa chỉ người dùng

        public DateTime? DateOfBirth { get; set; } // Ngày sinh (tuỳ chọn)

        [Required]
        public UserRole Role { get; set; } 

        public bool IsActive { get; set; } = true; // Kiểm tra tài khoản có đang hoạt động không

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Ngày tạo tài khoản

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Ngày cập nhật tài khoản lần cuối
        public string? EmailConfirmationCode { get; set; }


        // Quan hệ với các bảng khác
        public ICollection<Orders>? Orders { get; set; } // Một User có nhiều Orders
        public ICollection<Reviews>? Reviews { get; set; } // Một User có nhiều Reviews
        public ICollection<Cart> CartItems { get; set; } = new List<Cart>();// Một User có nhiều sản phẩm trong giỏ hàng 

        public ICollection<Address>? Addresses  { get; set; }


        public ICollection<FavoriteProducts> favoriteProducts { get; set; } = new List<FavoriteProducts>(); // Một User có nhiều sản phẩm yêu thích


        public ICollection<CompareProduct> CompareProducts { get; set; } = new List<CompareProduct>(); // Một User có nhiều sản phẩm so sánh

    }

    public enum UserRole
    {
        Customer = 1,
        Admin = 2
    }
}
