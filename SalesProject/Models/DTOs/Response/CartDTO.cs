using SalesProject.Models.DTOs;

namespace SalesProject.Models.DTO
{
    public class CartDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }


        public UserDto User { get; set; } = new UserDto();
        public string UserName { get; set; } = string.Empty;  // Assuming we want to include the user's name in the response
        public Guid ProductId { get; set; }

   
        public string ProductName { get; set; } =string.Empty; // Assuming we want to include the product's name in the response

        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
