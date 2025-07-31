namespace HoneyShop.ViewModels.Admin.OrderManagment
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string OrderStatus { get; set; } = null!;

        public Guid OrderStatusId { get; set; }

        public string UserName { get; set; } = null!;

        public string UserEmail { get; set; } = null!;

        public string ShippingCity { get; set; } = null!;

        public string ShippingAddress { get; set; } = null!;
    }
}
