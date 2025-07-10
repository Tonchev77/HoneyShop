namespace HoneyShop.Services.Core
{
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Shop;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<IEnumerable<GetAllProductsViewModel>> GetAllProductsAsync()
        {
           IEnumerable<GetAllProductsViewModel> allProducts = await this.productRepository
                .GetAllAttached()
                .Select(p => new GetAllProductsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                })
                .ToArrayAsync();

            return allProducts;
        }
    }
}
