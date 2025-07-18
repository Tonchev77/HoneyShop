namespace HoneyShop.Services.Core.ViewComponents
{
    using HoneyShop.Services.Core.Contracts;
    using Microsoft.AspNetCore.Mvc;
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICategoriesViewComponentService categoriesService;
        public CategoriesViewComponent(ICategoriesViewComponentService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await categoriesService.GetAllCategoriesAsync();
            return View(categories); // This will look for Views/Shared/Components/Categories/Default.cshtml
        }
    }
}
