using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesProject.Models.Domain
{
    public class OrderDiscounts
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Orders? Order { get; set; }

        public Guid DiscountId { get; set; }
        [ForeignKey("DiscountId")]
        public Discounts? Discount { get; set; }
    }
}
