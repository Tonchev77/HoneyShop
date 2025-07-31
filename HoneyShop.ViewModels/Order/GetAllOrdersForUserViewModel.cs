namespace HoneyShop.ViewModels.Order
{
    public class GetAllOrdersForUserViewModel
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string OrderStatus { get; set; } = null!;

        public string ShippingCity { get; set; } = null!;

        public string ShippingAddress { get; set; } = null!;

        public IEnumerable<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
    }
}
