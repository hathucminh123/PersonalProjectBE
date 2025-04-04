using SalesProject.Models.Domain;
using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
namespace SalesProject.Models.Domain;
    public class Products
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        // 🔥 Giá gốc trước khi giảm giá
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }

        // 🔥 Số tiền được giảm (thay vì giá sau giảm)
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountAmount { get; set; }

        // 🔹 Giá cuối cùng sau khi trừ số tiền giảm giá
        [NotMapped]
        public decimal FinalPrice => OriginalPrice - (DiscountAmount ?? 0);

        // 🔹 Tính phần trăm giảm giá dựa trên số tiền giảm
        [NotMapped]
        public int DiscountPercentage
        {
            get
            {
                if (OriginalPrice > 0 && DiscountAmount.HasValue && DiscountAmount.Value > 0)
                {
                    return (int)Math.Round((DiscountAmount.Value / OriginalPrice) * 100);
                }
                return 0;
            }
        }

        // 🔥 Đánh dấu sản phẩm là BestSeller nếu giảm giá >= 40%
        public bool IsBestSeller { get; set; }

        public int Stock { get; set; }

        public string Tags { get; set; } = string.Empty;

        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        [Required]
        public Guid SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; } 

        // 🔹 Hiệu ứng da đặc biệt (Ví dụ: "Hiệu Ứng Da Trắng Gương", "Làn Da Căng Bóng", v.v.)
        [MaxLength(100)]
        public string SkinEffect { get; set; } = string.Empty;


        public string ActiveIngredients { get; set; } = string.Empty;

        // 🔥 EXP - HSD Chưa Mở Nắp (Tháng)
        public int ExpShelfLife { get; set; }

        // 🔥 PAO - HSD Đã Mở Nắp (Tháng)
        public int PaoShelfLife { get; set; }

        // 🔥 Loại Da
        [MaxLength(255)]
        public string SkinType { get; set; } = string.Empty;

        // 🔥 Công Dụng Chính
        [MaxLength(1000)]
        public string Benefits { get; set; } = string.Empty;

        // 🔥 Thành Phần Chi Tiết
        [MaxLength(3000)]
        public string Ingredients { get; set; } = string.Empty;

        // 🔥 Công Dụng Chính (Tóm tắt nhanh)
        [MaxLength(1000)]
        public string MainBenefits { get; set; } = string.Empty;

        // 🔥 Vấn Đề Về Da
        [MaxLength(1000)]
        public string SkinConcerns { get; set; } = string.Empty;

        // 🔥 Cập nhật trạng thái sản phẩm
        public void UpdateProductStatus()
        {
            // ✅ Đánh dấu là "Best Seller" nếu giảm giá >= 40%
            if (DiscountPercentage >= 40)
            {
                IsBestSeller = true;
                Tags = Tags != null ? Tags + ", Best Seller" : "Best Seller";
            }
            else
            {
                IsBestSeller = false;
            }

            // ✅ Gắn tag "Sale" nếu có giảm giá
            if (DiscountAmount.HasValue && DiscountAmount.Value > 0)
            {
                Tags = Tags != null ? Tags + ", Sale" : "Sale";
            }
        }

    public ICollection<FavoriteProducts>? favoriteProducts { get; set; }

    public ICollection<CompareProduct> CompareProducts { get; set; } = new List<CompareProduct>();
}
