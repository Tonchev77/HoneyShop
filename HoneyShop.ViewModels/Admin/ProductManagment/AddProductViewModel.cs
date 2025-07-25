namespace HoneyShop.ViewModels.Admin.ProductManagment
{
    using HoneyShop.ViewModels.Admin.CategoryManagment;
    using HoneyShop.ViewModels.Home;
    using System.ComponentModel.DataAnnotations;

    using static HoneyShop.GCommon.ValidationConstants.Product;

    public class AddProductViewModel
    {
        [Required]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMinLengthMessage)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string Description { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        public virtual IEnumerable<AddProductCategoryDropDownModel>? Categories { get; set; }
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthMessage)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
