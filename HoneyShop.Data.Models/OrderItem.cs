namespace HoneyShop.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("A line item within an order")]
    public class OrderItem
    {
        [Comment("Order identifier")]
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        [Comment("Product identifier")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Comment("Quantity of the product ordered")]
        public int Quantity { get; set; }

        [Comment("Unit price of the product at the time of order")]
        public decimal UnitPrice { get; set; }

        [Comment("Shows if the item has been deleted")]
        public bool IsDeleted { get; set; }

        [Comment("Shows the date of deleting the item")]
        public DateTime? DeletedAt { get; set; }
    }
}
