namespace HoneyShop.Controllers
{
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Shop;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
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

        [AllowAnonymous]
        public async Task<IActionResult> DetailsProduct(Guid? id)
        {
            try
            {
                GetProductDetailViewModel? productDetails = await this.productService
                    .GetProductDetailAsync(id);

                if (productDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View("DetailsProduct", productDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
