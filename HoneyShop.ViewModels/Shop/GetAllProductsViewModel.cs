namespace HoneyShop.ViewModels.Shop
{
    public class GetAllProductsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        //public Guid CategoryId { get; set; }

        public string ImageUrl { get; set; } = null!;

    }
}
