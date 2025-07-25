namespace HoneyShop.ViewModels.Admin.ProductManagment
{
    using HoneyShop.ViewModels.Home;
    public class ProductManagmentIndexViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public virtual GetAllCategoriesViewModel Category { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
