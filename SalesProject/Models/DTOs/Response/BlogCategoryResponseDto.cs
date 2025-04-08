namespace SalesProject.Models.DTOs.Response
{
    public class BlogCategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        public List<BlogSubCategoryResponseDto>? SubCategories { get; set; }
    }
}
