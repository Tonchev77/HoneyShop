namespace HoneyShop.ViewModels.Admin.ProductManagment
{
    using HoneyShop.ViewModels.Home;

    public class DeleteProductManagmentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
