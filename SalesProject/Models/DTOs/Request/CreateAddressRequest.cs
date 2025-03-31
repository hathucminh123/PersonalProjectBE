namespace SalesProject.Models.DTOs.Request
{
    public class CreateAddressRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;

        public string? Email { get; set; }

        // Nếu client có gửi, thì set default luôn
        public bool IsDefault { get; set; } = false;
    }
}
