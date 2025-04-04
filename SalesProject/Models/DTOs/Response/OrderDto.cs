using SalesProject.Models.Domain;

namespace SalesProject.Models.DTOs.Response
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public UserDto User { get; set; } = new UserDto();
        public decimal TotalPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;



        public string PaymentStatus { get; set; } = "Pending";

        public AddressResponse? ShippingAddress { get; set; }


        public string? PaymentMethod { get; set; } 

        public DateTime CreatedAt { get; set; }

        // ✅ Danh sách các chi tiết đơn hàng
        public List<OrderDetailsDto>? OrderDetails { get; set; }

        // ✅ Danh sách mã giảm giá được áp dụng
        public List<DiscountDto>? Discounts { get; set; }
    }
}
