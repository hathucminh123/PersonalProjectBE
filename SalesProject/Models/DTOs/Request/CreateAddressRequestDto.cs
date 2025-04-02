namespace SalesProject.Models.DTOs.Request
{
    public class CreateAddressRequestDto
    {

        
        public Guid UserId { get; set; }

        // Thông tin người nhận hàng
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Địa chỉ chi tiết
        public string Province { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;

        // Email nhận thông báo (không bắt buộc)
        public string? Email { get; set; }

        // Đánh dấu địa chỉ mặc định
        public bool IsDefault { get; set; }
    }
}
