using SalesProject.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace SalesProject.Models.DTOs
{
    public class CategoriesDto
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true; // Danh mục có đang hoạt động không?

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Ngày tạo danh mục

        // 🔹 Quan hệ 1-Nhiều: Một Category có nhiều Products
        public ICollection<Products>? Products { get; set; }
    }
}
