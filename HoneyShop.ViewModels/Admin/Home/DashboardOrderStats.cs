namespace HoneyShop.ViewModels.Admin.Home
{
    public class DashboardOrderStats
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ConfirmedOrders { get; set; }
        public int SentOrders { get; set; }
        public int FinishedOrders { get; set; }
        public decimal TotalSales { get; set; }
        public decimal MonthlySales { get; set; }
        public decimal WeeklySales { get; set; }
        public decimal DailySales { get; set; }
    }
}
