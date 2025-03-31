namespace SalesProject.Models.DTOs.Response
{
    public class SubCategoryResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public Guid CategoryId { get; set; }

        public string? CategoryName { get; set; } // lấy tên danh mục cha nếu muốn hiển thị

        public int ProductCount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
