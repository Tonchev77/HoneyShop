namespace HoneyShop.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Association between a product and a promotion")]
    public class ProductPromotion
    {
        [Comment("Product identifier")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Comment("Promotion identifier")]
        public Guid PromotionId { get; set; }
        public Promotion Promotion { get; set; } = null!;

        [Comment("Shows if the promotion has been deleted")]
        public bool IsDeleted { get; set; }

        [Comment("Shows the date of deleting the promotion")]
        public DateTime? DeletedAt { get; set; }
    }
}
