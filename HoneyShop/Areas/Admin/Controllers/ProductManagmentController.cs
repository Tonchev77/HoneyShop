namespace HoneyShop.Areas.Admin.Controllers
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.CategoryManagment;
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

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? productId)
        {
            try
            {
                EditProductManagmentViewModel? editInputModel = await this.productService
                    .GetProductForEditingAsync(productId);

                if (editInputModel == null)
                {
                    TempData[ErrorMessageKey] = ProductNotFound;
                    return this.RedirectToAction(nameof(Index));
                }

                editInputModel.Categories = await this.categoryService
                    .GetCategoryDropdownDataAsync();

                return this.View(editInputModel);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = ProductFatalError;
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductManagmentViewModel inputModel)
        {
            try
            {
                bool editResult = false;

                if (!this.ModelState.IsValid)
                {
                    inputModel.Categories = await this.categoryService.GetCategoryDropdownDataAsync();
                    return this.View(inputModel);
                }

                editResult = await this.productService.PersistUpdateProductAsync(inputModel);
                if (editResult == false)
                {

                    TempData[ErrorMessageKey] = ProductEditError;
                    this.ModelState.AddModelError(string.Empty, ProductEditError);
                    return this.View(inputModel);
                }
                TempData[SuccessMessageKey] = ProductEditedSuccessfully;
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
