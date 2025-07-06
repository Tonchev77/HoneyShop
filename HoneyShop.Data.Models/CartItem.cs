namespace HoneyShop.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Product entry in a user's cart")]
    public class CartItem
    {
        [Comment("Foreign key to the cart")]
        public Guid CartId { get; set; }
        public Cart Cart { get; set; } = null!;

        [Comment("Foreign key to the product")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Comment("Number of product units added to the cart")]
        public int Quantity { get; set; }

        [Comment("Shows if the product in cart has been deleted")]
        public bool IsDeleted { get; set; }

        [Comment("Shows the date of deleting the product of cart")]
        public DateTime? DeletedAt { get; set; }
    }
}
