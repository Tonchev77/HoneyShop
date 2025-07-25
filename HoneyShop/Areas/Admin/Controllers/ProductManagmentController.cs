namespace HoneyShop.Areas.Admin.Controllers
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.ProductManagment;
    using Microsoft.AspNetCore.Mvc;
    public class ProductManagmentController : BaseAdminController
    {
        private readonly IProductService productService;

        public ProductManagmentController(IProductService productService)
        {
            this.productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<ProductManagmentIndexViewModel> allProducts = await
                    this.productService.GetAllProductsAsync();

                return View(allProducts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
