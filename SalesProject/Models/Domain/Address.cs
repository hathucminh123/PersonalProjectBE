namespace SalesProject.Models.Domain
{
    public class Address
    {
        public Guid Id { get; set; }

        // Liên kết với người dùng
        public Guid UserId { get; set; }
        public Users? User { get; set; }

        // Thông tin người nhận hàng
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty ;

        // Địa chỉ chi tiết
        public string Province { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string StreetAddress { get; set; } =string.Empty;

        // Email nhận thông báo (không bắt buộc)
        public string? Email { get; set; }

        // Đánh dấu địa chỉ mặc định
        public bool IsDefault { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
