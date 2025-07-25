namespace HoneyShop.Areas.Admin.Controllers
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.CategoryManagment;
    using Microsoft.AspNetCore.Mvc;
    
    using static HoneyShop.GCommon.ApplicationConstants;
    using static HoneyShop.GCommon.NotificationMessages.Category;

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
        public IActionResult Add()
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? categoryId)
        {
            try
            {

                EditCategoryManagmentViewModel? editInputModel = await this.categoryService
                    .GetCategoryForEditingAsync(categoryId);

                if (editInputModel == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(editInputModel);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryManagmentViewModel inputModel)
        {
            try
            {
                bool editResult = false;

                if (!this.ModelState.IsValid)
                {
                    return this.View(inputModel);
                }

                editResult = await this.categoryService.PersistUpdateCategoryAsync(inputModel);
                if (editResult == false)
                {

                    TempData[ErrorMessageKey] = CategoryError;
                    this.ModelState.AddModelError(string.Empty, CategoryError);
                    return this.View(inputModel);
                }
                TempData[SuccessMessageKey] = CategoryEditedSuccessfully;
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = CategoryError;
                return this.RedirectToAction(nameof(Index));

            }
        }
    }
}
