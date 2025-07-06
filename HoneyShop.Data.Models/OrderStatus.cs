namespace HoneyShop.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Defines statuses for an order")]
    public class OrderStatus
    {
        [Comment("Status identifier")]
        public Guid Id { get; set; }

        [Comment("Name of the order status")]
        public string Name { get; set; } = null!;

        [Comment("Optional description of the status")]
        public string? Description { get; set; }

        [Comment("Shows if the status has been deleted")]
        public bool IsDeleted { get; set; }

        [Comment("Shows the date of deleting the status")]
        public DateTime? DateleteAt { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
            = new HashSet<Order>();
    }
}
