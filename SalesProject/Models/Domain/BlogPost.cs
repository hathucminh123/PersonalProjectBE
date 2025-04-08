namespace SalesProject.Models.Domain
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; } =string.Empty;

        public string ImageUrl {  get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // FK đến BlogSubCategory
        public int BlogSubCategoryId { get; set; }
        public BlogSubCategory? BlogSubCategory { get; set; }
    }
}
