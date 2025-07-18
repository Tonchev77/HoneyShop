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
        public async Task<IActionResult> Index(string? searchString, Guid? categoryId)
        {
            //ShopIndexViewModel viewModel = new ShopIndexViewModel
            //{
            //    Products = await this.productService.GetAllProductsAsync(),
            //    Categories = await this.categoryService.GetAllCategoriesAsync()
            //};

            ////IEnumerable<GetAllProductsViewModel> allProducts = await this.productService.GetAllProductsAsync();

            ////IEnumerable<GetProductCategoryViewModel> allCategories = await this.categoryService.GetAllCategoriesAsync();

            //return View(viewModel);
            try
            {
                IEnumerable<GetAllProductsViewModel> products;

                // Filtering logic
                if (!string.IsNullOrWhiteSpace(searchString) && categoryId.HasValue)
                {
                    products = await this.productService.GetAllProductsByStringAndCategoryAsync(searchString, categoryId.Value);
                }
                else if (!string.IsNullOrWhiteSpace(searchString))
                {
                    products = await this.productService.GetAllProductsByStringAsync(searchString);
                }
                else if (categoryId.HasValue)
                {
                    products = await this.productService.GetAllProductsByCategoryAsync(categoryId);
                }
                else
                {
                    products = await this.productService.GetAllProductsAsync();
                }

                // Redirect if no products found
                if (!products.Any())
                {
                    TempData["Message"] = "No products found, showing all products.";
                    // Redirect to Index without filters
                    return RedirectToAction(nameof(Index), new { searchString = (string)null, categoryId = (Guid?)null });
                }

                var viewModel = new ShopIndexViewModel
                {
                    Products = products,
                    Categories = await this.categoryService.GetAllCategoriesAsync(),
                    SearchString = searchString,
                    CategoryId = categoryId
                };

                return View(viewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
            
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

        public async Task<IActionResult> Search(string searchString, Guid? id) 
        {
            try
            {
                IEnumerable<GetAllProductsViewModel> products;

                if (!string.IsNullOrWhiteSpace(searchString) && id.HasValue)
                {
                    // Filter both by search string and category
                    products = await productService.GetAllProductsByStringAndCategoryAsync(searchString, id.Value);
                }
                else if (!string.IsNullOrWhiteSpace(searchString))
                {
                    // Filter by search string only
                    products = await productService.GetAllProductsByStringAsync(searchString);
                }
                else if (id.HasValue)
                {
                    // Filter by category only
                    products = await productService.GetAllProductsByCategoryAsync(id);
                }
                else
                {
                    // No filter, show all products
                    products = await productService.GetAllProductsByStringAsync(null);
                }

                // Pass products to the view
                return View(products);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index), "Shop");
            }
        }
    }
}
