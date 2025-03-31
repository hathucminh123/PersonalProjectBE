namespace SalesProject.Models.DTOs.Response
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Danh sách sản phẩm thuộc danh mục (nếu cần)
        public List<ProductDto>? Products { get; set; }
    }
}
