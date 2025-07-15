namespace HoneyShop.ViewModels.Cart
{
    public class GetProductDetailsForCartViewModel
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
    }
}
