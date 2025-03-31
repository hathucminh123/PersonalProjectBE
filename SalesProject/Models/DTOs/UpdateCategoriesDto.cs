using System.ComponentModel.DataAnnotations;

namespace SalesProject.Models.DTOs
{
    public class UpdateCategoriesDto
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên danh mục tối đa 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "Mô tả tối đa 255 ký tự")]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
