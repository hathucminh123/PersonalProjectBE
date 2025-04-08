namespace SalesProject.Models.DTOs.Response
{
    public class BlogPostResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public int BlogSubCategoryId { get; set; }
        public string? BlogSubCategoryName { get; set; }
    }
}
