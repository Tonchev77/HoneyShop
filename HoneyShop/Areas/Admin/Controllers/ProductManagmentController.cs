namespace HoneyShop.Areas.Admin.Controllers
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.ProductManagment;
    using Microsoft.AspNetCore.Mvc;

    using static HoneyShop.GCommon.ApplicationConstants;
    using static HoneyShop.GCommon.NotificationMessages.Product;
    public class ProductManagmentController : BaseAdminController
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public ProductManagmentController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
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

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                AddProductViewModel inputModel = new AddProductViewModel()
                {
                    CreatedAt = DateTime.UtcNow,
                    Categories = await this.categoryService.GetCategoryDropdownDataAsync(),
                };

                return this.View(inputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = ProductFatalError;

                return this.RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    inputModel.Categories = await this.categoryService.GetCategoryDropdownDataAsync();

                    return this.View(inputModel);
                }

                bool addResult = await this.productService
                    .AddProductAsync(this.GetUserId()!, inputModel);

                if (addResult == false)
                {
                    ModelState.AddModelError(string.Empty, ProductFatalError);
                    TempData[ErrorMessageKey] = ProductFatalError;

                    return this.View(inputModel);
                }

                TempData[SuccessMessageKey] = ProductAddedSuccessfully;
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = ProductFatalError;

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
