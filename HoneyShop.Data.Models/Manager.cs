namespace HoneyShop.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Manager in the system")]
    public class Manager
    {
        [Comment("Manager identifier")]
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        [Comment("Manager's user entity")]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } 
            = new HashSet<Product>();
        public virtual ICollection<Category> Categories { get; set; } 
            = new HashSet<Category>();
        public virtual ICollection<OrderStatus> OrderStatuses { get; set; } 
            = new HashSet<OrderStatus>();
        public virtual ICollection<Warehouse> Warehouses { get; set; } 
            = new HashSet<Warehouse>();
    }
}
