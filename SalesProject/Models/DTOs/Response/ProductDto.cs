using SalesProject.Models.DTOs.Response;
using System;

namespace SalesProject.Models.DTOs
{
    public class ProductDto
    {
        // 🔹 ID sản phẩm
        public Guid Id { get; set; }

        // 🔹 Tên sản phẩm
        public string Name { get; set; } = string.Empty;

        // 🔹 Mô tả sản phẩm
        public string Description { get; set; } = string.Empty;

        // 🔹 Giá gốc
        public decimal OriginalPrice { get; set; }

        // 🔹 Số tiền được giảm (nếu có)
        public decimal? DiscountAmount { get; set; }

        // 🔹 Giá cuối cùng sau giảm giá
        public decimal FinalPrice { get; set; }

        // 🔹 Phần trăm giảm giá
        public int DiscountPercentage { get; set; }

        // 🔹 Best Seller
        public bool IsBestSeller { get; set; }

        // 🔹 Số lượng sản phẩm trong kho
        public int Stock { get; set; }

        // 🔹 Danh sách tags (Best Seller, Sale, etc.)
        public string Tags { get; set; } = string.Empty;

        // 🔹 Link hình ảnh sản phẩm
        public string ImageUrl { get; set; } = string.Empty;

        // 🔹 Ngày tạo sản phẩm
        public DateTime CreatedAt { get; set; }

        // 🔹 ID của danh mục con
        public Guid SubCategoryId { get; set; }


        public SubCategoryDTO SubCategory { get; set; } = new SubCategoryDTO();

        // 🔹 Tên danh mục con (nếu cần hiển thị)
        public string? SubCategoryName { get; set; }

        // 🔹 Hiệu ứng da đặc biệt (Làn da căng bóng, trắng gương,...)
        public string SkinEffect { get; set; } = string.Empty;

        // 🔹 Hoạt chất chính
        public string ActiveIngredients { get; set; } = string.Empty;

        // 🔹 Hạn sử dụng chưa mở nắp (tháng)
        public int ExpShelfLife { get; set; }

        // 🔹 Hạn sử dụng sau khi mở nắp (tháng)
        public int PaoShelfLife { get; set; }

        // 🔹 Loại da phù hợp
        public string SkinType { get; set; } = string.Empty;

        // 🔹 Công dụng chính
        public string Benefits { get; set; } = string.Empty;

        // 🔹 Thành phần chi tiết
        public string Ingredients { get; set; } = string.Empty;

        // 🔹 Công dụng chính (Tóm tắt nhanh)
        public string MainBenefits { get; set; } = string.Empty;

        // 🔹 Vấn đề về da được giải quyết
        public string SkinConcerns { get; set; } = string.Empty;
    }
}
