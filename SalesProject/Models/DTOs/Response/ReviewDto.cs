namespace SalesProject.Models.DTOs.Response
{
    public class ReviewDto
    {

        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // Thông tin về người dùng (nếu cần)
        public UserDto? User { get; set; }

        // Thông tin về sản phẩm (nếu cần)
        public ProductDto? Product { get; set; }
    }
}
