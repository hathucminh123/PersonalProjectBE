using System;
using System.ComponentModel.DataAnnotations;

namespace SalesProject.Models.Domain
{
    public class CompareProduct
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public Users? User { get; set; }


        [Required]
        public Guid ProductId { get; set; }


        public Products? Products { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
