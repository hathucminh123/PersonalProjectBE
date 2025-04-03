namespace SalesProject.Models.DTOs.Response
{
    public class OrderDetailsDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }


        public string ImageUrl { get; set; } = string.Empty;

        // Thêm set nếu bạn muốn gán giá trị từ entity
        public decimal TotalPrice { get; set; }
    }


}
