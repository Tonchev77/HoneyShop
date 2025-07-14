namespace HoneyShop.Services.Core
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Shop;
    using Microsoft.EntityFrameworkCore;
    using System;
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

        public async Task<GetProductDetailViewModel?> GetProductDetailAsync(Guid? id)
        {
            GetProductDetailViewModel? detailsVm = null;
            if (id.HasValue)
            {
                Product? productModel = await this.productRepository
                    .SingleOrDefaultAsync(p => p.Id == id.Value);

                if (productModel != null)
                {
                    detailsVm = new GetProductDetailViewModel()
                    {
                        Id = productModel.Id,
                        Name = productModel.Name,
                        ImageUrl = productModel.ImageUrl,
                        Description = productModel.Description,
                        Price = productModel.Price,
                        IsActive = productModel.IsActive,
                        //Category = productModel.Category.Name,
                    };
                }
            }
            return detailsVm;
        }
    }
}
