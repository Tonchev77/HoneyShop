namespace HoneyShop.ViewModels.Order
{
    public class OrderItemViewModel
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice => ProductPrice * Quantity;
    }
}
