namespace HoneyShop.Services.Core.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.ProductManagment;
    using HoneyShop.ViewModels.Home;
    using Microsoft.EntityFrameworkCore;

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
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
