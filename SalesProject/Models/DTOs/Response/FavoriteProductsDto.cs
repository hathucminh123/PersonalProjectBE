namespace SalesProject.Models.DTOs.Response
{
    public class FavoriteProductsDto
    {


        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ProductDto? Product { get; set; } // Một FavoriteProducts thuộc về một Product
        public UserDto? User { get; set; } // Một FavoriteProducts thuộc về một User
    }
}
