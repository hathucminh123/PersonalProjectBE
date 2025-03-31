namespace SalesProject.Models.DTOs.Response
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<SubCategoryResponse> SubCategories { get; set; } = new List<SubCategoryResponse>();
    }
}
