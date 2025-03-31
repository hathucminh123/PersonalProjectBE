﻿using System.ComponentModel.DataAnnotations;

namespace SalesProject.Models.DTOs.Request
{
    public class CategoryUpdateRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
