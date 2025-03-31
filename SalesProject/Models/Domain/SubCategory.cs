
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SalesProject.Models.Domain;
public class SubCategory
{
    [Key]
    public Guid Id { get; set; }

    // Tên danh mục phụ
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    // Mô tả danh mục phụ
    [MaxLength(500)]
    public string Description { get; set; } =string.Empty;

    // Trạng thái hoạt động
    public bool IsActive { get; set; } = true;

    // Liên kết với Category
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    // Danh sách các sản phẩm thuộc SubCategory này
    public ICollection<Products> Products { get; set; } = new List<Products>();

    // Đếm số lượng sản phẩm thuộc SubCategory này
    public int ProductCount
    {
        get
        {
            return Products?.Count ?? 0;
        }
    }

    public DateTime CreatedAt { get; set; }
}
