namespace HoneyShop.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    public class ApplicationUser : IdentityUser
    {
        public virtual Manager? Manager { get; set; }

        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Cart> Carts { get; set; } 
            = new HashSet<Cart>();
        public virtual ICollection<Order> Orders { get; set; } 
            = new HashSet<Order>();
        public virtual ICollection<Product> CreatedProducts { get; set; } 
            = new HashSet<Product>();
    }
}
