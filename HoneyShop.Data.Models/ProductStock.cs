namespace HoneyShop.Data.Models
{
    using Microsoft.EntityFrameworkCore;


    [Comment("Inventory of a product stored in a specific warehouse")]
    public class ProductStock
    {
        [Comment("Product identifier")]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        [Comment("Warehouse identifier")]
        public Guid WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; } = null!;

        [Comment("Current quantity of the product in stock at the warehouse")]
        public int Quantity { get; set; }

        [Comment("Shows if the item has been deleted")]
        public bool IsDeleted { get; set; }

        [Comment("Shows the date of deleting the item")]
        public DateTime? DeletedAt { get; set; }
    }
}
