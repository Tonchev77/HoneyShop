namespace HoneyShop.Controllers
{
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Shop;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class ShopController : BaseController
    {
        private readonly IProductService productService;

        public ShopController(IProductService productService)
        {
            this.productService = productService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
           IEnumerable<GetAllProductsViewModel> allProducts = await this.productService.GetAllProductsAsync();
            return View(allProducts);
        }
    }
}
