using SalesProject.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace SalesProject.Models.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;


        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } // Role cho phép chọn Customer hoặc Admin
    }
}
