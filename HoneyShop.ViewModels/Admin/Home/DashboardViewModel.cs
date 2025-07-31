namespace HoneyShop.ViewModels.Admin.Home
{
    public class DashboardViewModel
    {
        // Order statistics
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ConfirmedOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int CompletedOrders { get; set; }

        // Sales statistics
        public decimal TotalSales { get; set; }
        public decimal MonthlySales { get; set; }
        public decimal WeeklySales { get; set; }
        public decimal DailySales { get; set; }

        // Inventory statistics
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }

        // Customer statistics
        public int TotalCustomers { get; set; }
        public int NewCustomersThisMonth { get; set; }

        // Recent activity
        public IEnumerable<RecentOrderViewModel> RecentOrders { get; set; } = new List<RecentOrderViewModel>();
        public IEnumerable<BestSellingProductViewModel> BestSellingProducts { get; set; } = new List<BestSellingProductViewModel>();

        // Warehouse statistics
        public int TotalWarehouses { get; set; }
        public IEnumerable<WarehouseStockSummaryViewModel> WarehouseSummaries { get; set; } = new List<WarehouseStockSummaryViewModel>();
    }
}
