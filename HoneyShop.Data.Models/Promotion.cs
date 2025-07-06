namespace HoneyShop.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Promotional offer applicable to one or more products")]
    public class Promotion
    {
        [Comment("Promotion identifier")]
        public Guid Id { get; set; }

        [Comment("Name of the promotion")]
        public string Name { get; set; } = null!;

        [Comment("Detailed description of the promotion")]
        public string Description { get; set; } = null!;

        [Comment("Discount percentage applied by the promotion")]
        public decimal DiscountPercentage { get; set; }

        [Comment("Date when the promotion becomes active")]
        public DateTime StartDate { get; set; }

        [Comment("Date when the promotion expires")]
        public DateTime EndDate { get; set; }

        [Comment("Shows the date of deleting the promotion")]
        public bool IsDeleted { get; set; }

        [Comment("Shows the date of deleting the promotion")]
        public DateTime? DeletedAt { get; set; }
        public virtual ICollection<ProductPromotion> ProductPromotions { get; set; }
               = new HashSet<ProductPromotion>();
    }
}
