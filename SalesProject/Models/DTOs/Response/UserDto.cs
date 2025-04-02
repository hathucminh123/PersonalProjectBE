using SalesProject.Models.Domain;
using SalesProject.Models.DTO;
using SalesProject.Models.DTOs.Response;

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? GoogleId { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Danh sách quan hệ
    public List<OrderDto>? Orders { get; set; }
    public List<CartDTO>? CartItems { get; set; }
    public List<ReviewDto>? Reviews { get; set; }

    public List<AddressResponse>? Addresses { get; set; }
}
