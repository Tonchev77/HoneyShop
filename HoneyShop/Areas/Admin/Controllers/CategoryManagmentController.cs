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
                TempData[ErrorMessageKey] = CategoryFatalError;
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
                    TempData[ErrorMessageKey] = CategoryEditError;
                    ModelState.AddModelError(string.Empty, CategoryEditError);
                    return this.View(inputModel);
                }

                TempData[SuccessMessageKey] = CategoryAddedSuccessfully;
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = CategoryFatalError;
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
                    TempData[ErrorMessageKey] = CategoryNotFound;
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(editInputModel);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = CategoryFatalError;
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

                    TempData[ErrorMessageKey] = CategoryEditError;
                    this.ModelState.AddModelError(string.Empty, CategoryEditError);
                    return this.View(inputModel);
                }
                TempData[SuccessMessageKey] = CategoryEditedSuccessfully;
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = CategoryFatalError;
                return this.RedirectToAction(nameof(Index));

            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? categoryId)
        {
            try
            {
                DeleteCategoryManagmentViewModel? deleteInputModel = await this.categoryService
                    .GetCategoryForDeleteAsync(categoryId);

                if (deleteInputModel == null)
                {
                    TempData[ErrorMessageKey] = CategoryNotFound;
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(deleteInputModel);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = CategoryFatalError;
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(DeleteCategoryManagmentViewModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    TempData[ErrorMessageKey] = CategoryDeletedError;
                    ModelState.AddModelError(string.Empty, CategoryDeletedError);

                    return this.View(inputModel);
                }

                bool deleteResult = await this.categoryService.SoftDeleteCategoryAsync(inputModel);

                if (deleteResult == false)
                {
                    TempData[ErrorMessageKey] = CategoryDeletedError;
                    this.ModelState.AddModelError(string.Empty, CategoryDeletedError);

                    return this.View(inputModel);
                }
                
                TempData[SuccessMessageKey] = CategoryDeletedSuccessfully;
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = CategoryFatalError;

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
