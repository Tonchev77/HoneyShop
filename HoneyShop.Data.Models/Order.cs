namespace HoneyShop.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    [Comment("Orders in the system")]
    public class Order
    {
        [Comment("Order identifier")]
        public Guid Id { get; set; }

        [Comment("Identifier of the user who placed the order")]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        [Comment("Shows the date of ordering the product")]
        public DateTime OrderDate { get; set; }

        [Comment("Shows the total amount of order")]
        public decimal TotalAmount { get; set; }

        [Comment("Shows the status of order")]
        public Guid OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; } = null!;

        [Comment("Destination city for shipping the order")]
        public string ShippingCity { get; set; } = null!;

        [Comment("Street address for shipping the order")]
        public string ShippingAddress { get; set; } = null!;

        [Comment("Shows if the order has been deleted")]
        public bool IsDeleted { get; set; }

        [Comment("Shows the date of deleting the order")]
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
            = new HashSet<OrderItem>();
    }
}
