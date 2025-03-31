using System;
using System.Collections.Generic;

namespace SalesProject.Models.DTOs
{
    public class SubCategoryDTO
    {
        // Mã danh mục phụ
        public Guid Id { get; set; }

        // Tên danh mục phụ
        public string Name { get; set; } = string.Empty;

        // Mô tả danh mục phụ
        public string Description { get; set; } = string.Empty;

        // Trạng thái hoạt động
        public bool IsActive { get; set; }

        // Liên kết với CategoryId
        public Guid CategoryId { get; set; }

        // Tên danh mục chính (Category Name)
        public string CategoryName { get; set; } = string.Empty;

        // Đếm số lượng sản phẩm thuộc SubCategory này
        public int ProductCount { get; set; }

        // Ngày tạo danh mục phụ
        public DateTime CreatedAt { get; set; }
    }
}
