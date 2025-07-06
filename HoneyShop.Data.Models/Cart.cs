namespace HoneyShop.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    [Comment("Cart with user orders in the system")]
    public class Cart
    {
        [Comment("Cart identifier")]
        public Guid Id { get; set; }

        [Comment("User identifier")]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;
        public virtual ICollection<CartItem> Items { get; set; }
            = new HashSet<CartItem>();

        [Comment("Shows if the cart has been deleted")]
        public bool IsDeleted { get; set; }

        [Comment("Shows the date of deleting the cart")]
        public DateTime? DeletedAt { get; set; }
    }
}
