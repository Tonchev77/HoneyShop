namespace HoneyShop.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Represents a category to which products can belong")]
    public class Category
    {
        [Comment("Category identifier")]
        public Guid Id { get; set; }

        [Comment("Name of the category")]
        public string Name { get; set; } = null!;

        [Comment("Optional description of the category")]
        public string? Description { get; set; }

        [Comment("Shows if the category has been deleted")]
        public bool IsDeleted { get; set; }

        [Comment("Shows the date of deleting the category")]
        public DateTime? DateleteAt { get; set; }
        public virtual ICollection<Product> Products { get; set; }
            = new HashSet<Product>();
    }
}
