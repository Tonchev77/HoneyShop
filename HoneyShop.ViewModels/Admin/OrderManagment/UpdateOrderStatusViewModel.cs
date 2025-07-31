namespace HoneyShop.ViewModels.Admin.OrderManagment
{
    using System.ComponentModel.DataAnnotations;

    using static HoneyShop.GCommon.ValidationConstants.OrderStatus;
    public class UpdateOrderStatusViewModel
    {
        public Guid OrderId { get; set; }

        [Required(ErrorMessage = OrderStatusMessageRequired)]
        public Guid StatusId { get; set; }
    }
}
