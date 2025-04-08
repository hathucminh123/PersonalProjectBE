namespace SalesProject.Models.DTOs.Response
{
    public class BlogSubCategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        public int BlogCategoryId { get; set; }
        public string? BlogCategoryName { get; set; }

        // ✅ Thêm danh sách BlogPosts
        public List<BlogPostResponseDto>? BlogPosts { get; set; }
    }
}
