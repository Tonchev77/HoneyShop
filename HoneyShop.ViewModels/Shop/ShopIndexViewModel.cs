namespace HoneyShop.ViewModels.Shop
{
    using HoneyShop.ViewModels.Home;
    public class ShopIndexViewModel
    {
        public IEnumerable<GetAllProductsViewModel> Products { get; set; } = null!;
        public IEnumerable<GetAllCategoriesViewModel> Categories { get; set; } = null!;
        public string? SearchString { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
