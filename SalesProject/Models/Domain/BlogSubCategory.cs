namespace SalesProject.Models.Domain
{
    public class BlogSubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;  // VD: "Trị mụn"
        public string Slug { get; set; } = string.Empty;
        public int BlogCategoryId { get; set; }
        public BlogCategory? BlogCategory { get; set; }

        public ICollection<BlogPost>? BlogPosts { get; set; }
    }
}
