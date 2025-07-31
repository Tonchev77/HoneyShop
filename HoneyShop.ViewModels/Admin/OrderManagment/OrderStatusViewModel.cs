namespace HoneyShop.ViewModels.Admin.OrderManagment
{
    public class OrderStatusViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
