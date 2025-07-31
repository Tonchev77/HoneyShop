namespace HoneyShop.ViewModels.Admin.Home
{
    public class RecentOrderViewModel
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = null!;
    }
}
