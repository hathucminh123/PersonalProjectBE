namespace SalesProject.Models.DTOs.Request
{
    public class UserRequest
    {

        public string FullName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; }
    }
}
