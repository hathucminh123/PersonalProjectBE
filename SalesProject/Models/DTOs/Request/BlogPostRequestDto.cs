namespace SalesProject.Models.DTOs.Request
{
    public class BlogPostRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int BlogSubCategoryId { get; set; }
    }
}
