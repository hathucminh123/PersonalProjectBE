using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesProject.Models.Domain
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; } 

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public Users? User { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Products? Product { get; set; }

        public int Quantity { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool? IsActive { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
