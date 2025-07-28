namespace HoneyShop.ViewModels.Admin.WarehouseManagment
{
    public class GetProductsInWarehouseViewModel
    {
        public Guid WarehouseId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; } = null!;
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
