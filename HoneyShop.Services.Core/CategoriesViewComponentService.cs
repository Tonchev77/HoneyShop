namespace HoneyShop.Services.Core
{
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Home;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CategoriesViewComponentService : ICategoriesViewComponentService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesViewComponentService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<GetAllCategoriesViewModel>> GetAllCategoriesAsync()
        {
            IEnumerable<GetAllCategoriesViewModel> categories = await categoryRepository
           .GetAllAttached()
           .Select(c => new GetAllCategoriesViewModel
           {
               Id = c.Id,
               Name = c.Name
           })
           .ToListAsync();

            return categories;
        }
    }
}
