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

        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryManagmentIndexViewModel> categories = await this.categoryService.GetAllCategoriesAsync();
            return View(categories);
        }
    }
}
