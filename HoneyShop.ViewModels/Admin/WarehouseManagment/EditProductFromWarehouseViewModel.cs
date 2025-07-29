namespace HoneyShop.ViewModels.Admin.WarehouseManagment
{
    using HoneyShop.ViewModels.Admin.ProductManagment;
    using System.ComponentModel.DataAnnotations;
    public class EditProductFromWarehouseViewModel
    {
        [Required]
        public Guid WarehouseId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public IEnumerable<ProductManagmentIndexViewModel> Products { get; set; } = null!;
    }
}
