using SalesProject.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SalesProject.Models.Domain;
public class Category
{
    [Key]
    public Guid Id { get; set; }

    // Tên danh mục
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    // Mô tả danh mục
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    // Trạng thái danh mục (có đang được sử dụng hay không)
    public bool IsActive { get; set; } = true;

    // Danh sách SubCategory thuộc danh mục này
    public ICollection<SubCategory>? SubCategories { get; set; }

    // Tự động đếm số lượng sản phẩm thuộc danh mục này
    public int ProductCount
    {
        get
        {
            int count = 0;
            if (SubCategories != null)
            {
                foreach (var subCategory in SubCategories)
                {
                    count += subCategory.ProductCount;
                }
            }
            return count;
        }
    }

    public DateTime CreatedAt { get; set; }
}
