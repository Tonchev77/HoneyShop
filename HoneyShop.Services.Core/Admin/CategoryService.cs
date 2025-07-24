namespace HoneyShop.Services.Core.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.CategoryManagment;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryManagmentIndexViewModel>> GetAllCategoriesAsync()
        {
            List<Category> categories = await this.categoryRepository
            .GetAllAttached()
            .ToListAsync();

            IEnumerable<CategoryManagmentIndexViewModel> allCategories = categories
                .Select(p => new CategoryManagmentIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description!
                })
                .ToList();

            return allCategories;

        }
    }
}
