namespace HoneyShop.ViewModels.Admin.Home
{
    public class ProductStatisticsViewModel
    {
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
    }
}
