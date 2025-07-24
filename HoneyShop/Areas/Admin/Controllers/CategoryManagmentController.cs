namespace HoneyShop.Areas.Admin.Controllers
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.CategoryManagment;
    using Microsoft.AspNetCore.Mvc;
    public class CategoryManagmentController : BaseAdminController
    {
        private readonly ICategoryService categoryService;
        public CategoryManagmentController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryManagmentIndexViewModel> categories = await this.categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                AddCategoryViewModel inputModel = new AddCategoryViewModel()
                {
                    Name = string.Empty,
                    Description = string.Empty
                };

                return this.View(inputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryViewModel inputModel)
        {

            try
            {
                bool addResult = await this.categoryService
                    .AddCategoryAsync(inputModel);

                if (addResult == false)
                {
                    ModelState.AddModelError(string.Empty, "Fatal error occured while adding a category!");
                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
