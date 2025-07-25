namespace HoneyShop.ViewModels.Admin.CategoryManagment
{
    using System.ComponentModel.DataAnnotations;

    using static HoneyShop.GCommon.ValidationConstants.Category;
    public class AddCategoryViewModel
    {
        [Required]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string Description { get; set; } = null!;
    }
}
