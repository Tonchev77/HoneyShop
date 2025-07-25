namespace HoneyShop.Services.Core.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.ProductManagment;
    using HoneyShop.ViewModels.Home;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.userManager = userManager;
        }

        public async Task<bool> AddProductAsync(string userId, AddProductViewModel inputModel)
        {
            bool opResult = false;

            ApplicationUser? user = await this.userManager.FindByIdAsync(userId);

            Category? categoryRef = await this.categoryRepository
                .FirstOrDefaultAsync(c => c.Id == inputModel.CategoryId);

            if ((user != null) && (categoryRef != null))
            {
                Product newProduct = new Product()
                {
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Price = inputModel.Price,
                    ImageUrl = inputModel.ImageUrl,
                    CreatedAt = DateTime.UtcNow,
                    CreatorId = user.Id,
                    IsActive = inputModel.IsActive,
                    CategoryId = inputModel.CategoryId,
                };

                await this.productRepository.AddAsync(newProduct);
                await this.productRepository.SaveChangesAsync();

                opResult = true;
            }

            return opResult;
        }

        public async Task<IEnumerable<ProductManagmentIndexViewModel>> GetAllProductsAsync()
        {
            List<Product> products = await this.productRepository
            .GetAllAttached()
            .Include(p => p.Category)
            .ToListAsync();

            IEnumerable<ProductManagmentIndexViewModel> allProducts = products
                .Select(p => new ProductManagmentIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Category = new GetAllCategoriesViewModel()
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    },
                    CreatedAt = p.CreatedAt,
                    IsActive = p.IsActive,

                })
                .ToList();

            return allProducts;
        }
    }
}
