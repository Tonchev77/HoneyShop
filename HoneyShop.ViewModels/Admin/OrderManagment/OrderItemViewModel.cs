namespace HoneyShop.ViewModels.Admin.OrderManagment
{
    public class OrderItemViewModel
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
