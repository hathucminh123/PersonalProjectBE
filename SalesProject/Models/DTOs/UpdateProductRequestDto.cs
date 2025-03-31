using System;
using System.ComponentModel.DataAnnotations;

namespace SalesProject.Models.DTOs
{
    public class UpdateProductRequestDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0")]
        public decimal OriginalPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá khuyến mãi phải lớn hơn hoặc bằng 0")]
        public decimal? DiscountAmount { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng trong kho phải lớn hơn hoặc bằng 0")]
        public int Stock { get; set; }

        [Required]
        public Guid SubCategoryId { get; set; }

        [MaxLength(500)]
        [Url(ErrorMessage = "Đường dẫn không hợp lệ")]
        public string ImageUrl { get; set; } = string.Empty;

        [MaxLength(100)]
        public string SkinEffect { get; set; } = string.Empty;

        [MaxLength(500)]
        public string ActiveIngredients { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "Hạn sử dụng chưa mở nắp phải từ 0 đến 100 tháng")]
        public int ExpShelfLife { get; set; }

        [Range(0, 100, ErrorMessage = "Hạn sử dụng sau khi mở nắp phải từ 0 đến 100 tháng")]
        public int PaoShelfLife { get; set; }

        [MaxLength(255)]
        public string SkinType { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Benefits { get; set; } = string.Empty;

        [MaxLength(3000)]
        public string Ingredients { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string MainBenefits { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string SkinConcerns { get; set; } = string.Empty;

        public bool IsBestSeller { get; set; }
    }
}
