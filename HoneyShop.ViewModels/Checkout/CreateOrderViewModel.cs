using System.ComponentModel.DataAnnotations;
using static HoneyShop.GCommon.ValidationConstants.Order;

namespace HoneyShop.ViewModels.Order
{
    public class CreateOrderViewModel
    {
        [Required]
        [MinLength(ShippingCityMinLength, ErrorMessage = ShippingCityMinLengthMessage)]
        [MaxLength(ShippingCityMaxLength, ErrorMessage = ShippingCityMaxLengthMessage)]
        public string ShippingCity { get; set; } = null!;

        [Required]
        [MinLength(ShippingAddressMinLength, ErrorMessage = ShippingAddressMinLengthMessage)]
        [MaxLength(ShippingAddressMaxLength, ErrorMessage = ShippingAddressMaxLengthMessage)]
        public string ShippingAddress { get; set; } = null!;

        public decimal TotalAmount { get; set; }
    }
}
