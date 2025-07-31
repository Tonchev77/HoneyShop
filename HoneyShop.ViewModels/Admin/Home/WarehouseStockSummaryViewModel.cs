namespace HoneyShop.ViewModels.Admin.Home
{
    public class WarehouseStockSummaryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public int TotalProducts { get; set; }
        public int TotalStock { get; set; }
    }
}
