namespace SalesProject.Models.Domain
{
    public class BlogCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;   // VD: "Cách chăm da"
        public string Slug { get; set; } = string.Empty;     // VD: "cach-cham-da"
        public ICollection<BlogSubCategory>? SubCategories { get; set; }
    }
}
