namespace SalesProject.Models.DTOs.Response
{
    public class AddressResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;

        public string? Email { get; set; }

        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
