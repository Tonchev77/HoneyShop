namespace HoneyShop.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    [Comment("Products available in the catalog")]
    public class Product
    {
        [Comment("Product identifier")]
        public Guid Id { get; set; }

        [Comment("Name of the product")]
        public string Name { get; set; } = null!;

        [Comment("Detailed description of the product")]
        public string Description { get; set; } = null!;

        [Comment("Current selling price of the product")]
        public decimal Price { get; set; }

        [Comment("Category of the product")]
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        [Comment("URL of the product image")]
        public string ImageUrl { get; set; } = null!;

        [Comment("Identifier of the user who created the product")]
        public string CreatorId { get; set; } = null!;
        public virtual IdentityUser Creator { get; set; } = null!;

        [Comment("Shows the date of creating the product")]
        public DateTime CreatedAt { get; set; }

        [Comment("Indicates whether the product is currently active and visible")]
        public bool IsActive { get; set; }

        [Comment("Shows if the product has been deleted")]
        public bool IsDeleted { get; set; }

        [Comment("Shows the date of deleting the product")]
        public DateTime? DeletedAt { get; set; }

        public Guid? ManagerId { get; set; }
        public virtual ICollection<ProductStock> ProductStocks { get; set; }
            = new HashSet<ProductStock>();
        public virtual ICollection<ProductPromotion> ProductPromotions { get; set; }
            = new HashSet<ProductPromotion>();
        public virtual ICollection<OrderItem> OrderItems { get; set; }
            = new HashSet<OrderItem>();
    }
}
