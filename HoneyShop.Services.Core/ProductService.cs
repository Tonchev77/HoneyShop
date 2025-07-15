namespace HoneyShop.Services.Core
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Home;
    using HoneyShop.ViewModels.Shop;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<GetAllProductsViewModel>> GetAllProductsAsync()
        {
            List<Product> products = await this.productRepository
            .GetAllAttached()
            .Include(p => p.Category)
            .ToListAsync();

            IEnumerable<GetAllProductsViewModel> allProducts = products
                .Select(p => new GetAllProductsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Category = new GetAllCategoriesViewModel()
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    }
                })
                .ToList();

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
                    Guid categoryId = productModel.CategoryId;
                    Category? category = await categoryRepository.GetByIdAsync(categoryId);

                    if (category != null) 
                    {
                        detailsVm = new GetProductDetailViewModel()
                        {
                            Id = productModel.Id,
                            Name = productModel.Name,
                            ImageUrl = productModel.ImageUrl,
                            Description = productModel.Description,
                            Price = productModel.Price,
                            IsActive = productModel.IsActive,
                            Category = new GetAllCategoriesViewModel()
                            {
                                Id = category.Id,
                                Name = category.Name
                            }
                        };
                    }
                }
            }
            return detailsVm;
        }
    }
}
