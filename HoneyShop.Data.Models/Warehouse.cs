namespace HoneyShop.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Physical storage location for product inventory")]
    public class Warehouse
    {
        [Comment("Warehouse identifier")]
        public Guid Id { get; set; }

        [Comment("Name of the warehouse")]
        public string Name { get; set; } = null!;

        [Comment("Physical address or location of the warehouse")]
        public string Location { get; set; } = null!;

        [Comment("Shows if the warehouse has been deleted")]
        public bool IsDeleted { get; set; }

        [Comment("Shows the date of deleting the warehouse")]
        public DateTime? DeletedAt { get; set; }
        public Guid? ManagerId { get; set; }
        public virtual ICollection<ProductStock> ProductStocks { get; set; }
            = new HashSet<ProductStock>();
    }
}
