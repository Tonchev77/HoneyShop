namespace HoneyShop.ViewModels.Admin.WarehouseManagment
{
    using System.ComponentModel.DataAnnotations;

    using static HoneyShop.GCommon.ValidationConstants.Warehouse;
    public class AddWarehouseViewModel
    {
        [Required]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(LocationMinLength, ErrorMessage = LocationMinLengthMessage)]
        [MaxLength(LocationMaxLength, ErrorMessage = LocationMaxLengthMessage)]
        public string Location { get; set; } = null!;
    }
}
