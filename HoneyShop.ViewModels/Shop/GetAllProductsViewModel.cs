using HoneyShop.ViewModels.Home;

namespace HoneyShop.ViewModels.Shop
{
    public class GetAllProductsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public virtual GetAllCategoriesViewModel Category { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

    }
}
