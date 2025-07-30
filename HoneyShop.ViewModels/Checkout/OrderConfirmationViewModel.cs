namespace HoneyShop.ViewModels.Order
{
    public class OrderConfirmationViewModel
    {
        public Guid OrderId { get; set; }

        public string ShippingCity { get; set; } = null!;

        public string ShippingAddress { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderStatus { get; set; } = null!;
    }
}
