namespace HoneyShop.Controllers
{
    using HoneyShop.Services.Core;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Shop;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static HoneyShop.GCommon.ApplicationConstants;
    public class ShopController : BaseController
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        private const int DefaultPageSize = DefaultProductsPerPageSize; // Add this constant definition
        public ShopController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchString, Guid? categoryId, SortOption sortBy = SortOption.Default, int page = 1)
        {
            try
            {
                IEnumerable<GetAllProductsViewModel> products;

                // Filtering logic using ShopService methods
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

                // Create the view model with the filtered products and categories
                ShopIndexViewModel? viewModel = new ShopIndexViewModel
                {
                    Products = products,
                    Categories = await this.categoryService.GetAllCategoriesAsync(),
                };

                // Set filter and pagination options
                viewModel.SearchString = searchString;
                viewModel.CategoryId = categoryId;
                viewModel.SortBy = sortBy;
                viewModel.CurrentPage = page;
                viewModel.PageSize = DefaultPageSize;
                viewModel.TotalProducts = products.Count();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
