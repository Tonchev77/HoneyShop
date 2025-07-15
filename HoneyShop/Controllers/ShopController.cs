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
        private readonly ICategoryService categoryService;

        public ShopController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }

        
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ShopIndexViewModel viewModel = new ShopIndexViewModel
            {
                Products = await this.productService.GetAllProductsAsync(),
                Categories = await this.categoryService.GetAllCategoriesAsync()
            };

            //IEnumerable<GetAllProductsViewModel> allProducts = await this.productService.GetAllProductsAsync();

            //IEnumerable<GetProductCategoryViewModel> allCategories = await this.categoryService.GetAllCategoriesAsync();

            return View(viewModel);

            
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
