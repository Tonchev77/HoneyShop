namespace HoneyShop.Services.Core
{
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Home;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<GetAllCategoriesViewModel>> GetAllCategoriesAsync()
        {
            IEnumerable<GetAllCategoriesViewModel> allCategories = await this.categoryRepository
                .GetAllAttached()
                .Select(c => new GetAllCategoriesViewModel 
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToArrayAsync();
                
            return allCategories;
        }
    }
}
