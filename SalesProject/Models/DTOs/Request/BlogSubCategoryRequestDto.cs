namespace SalesProject.Models.DTOs.Request
{
    public class BlogSubCategoryRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int BlogCategoryId { get; set; }
    }
}
