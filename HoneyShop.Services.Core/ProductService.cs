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
    using System.Linq;
    using System.Threading.Tasks;

    using Product = Data.Models.Product;
    using Category = Data.Models.Category;

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

        public async Task<IEnumerable<GetAllProductsViewModel>> GetAllProductsByCategoryAsync(Guid? id)
        {
            IQueryable<Product> products = this.productRepository
            .GetAllAttached()
            .Include(p => p.Category);

            if (id.HasValue) 
            {
                products = products.Where(p => p.CategoryId == id.Value);
            }

            IEnumerable<GetAllProductsViewModel> allProducts = await products
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
                .ToListAsync();

            return allProducts;
        }

        public async Task<IEnumerable<GetAllProductsViewModel>> GetAllProductsByStringAndCategoryAsync(string searchString, Guid id)
        {
            IEnumerable<GetAllProductsViewModel> products = await this.productRepository
            .GetAllAttached()
            .Include(p => p.Category)
            .Where(p =>
            (p.Name.ToLower().Contains(searchString.ToLower())
                || p.Description.ToLower().Contains(searchString.ToLower()))
            && p.CategoryId == id)
            .Select(p => new GetAllProductsViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Category = new GetAllCategoriesViewModel
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name
                }
            })
            .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<GetAllProductsViewModel>> GetAllProductsByStringAsync(string? searchString)
        {
            IQueryable<Product> products =  this.productRepository
                .GetAllAttached()
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString)) 
            {
                string lowered = searchString.ToLower();

                products = products
                .Where(p => p.Name.ToLower().Contains(lowered)
                        || p.Description.ToLower().Contains(lowered));
            }

            IEnumerable<GetAllProductsViewModel> allProducts = await products
            .Select(p => new GetAllProductsViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Category = new GetAllCategoriesViewModel
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name
                }
            })
            .ToListAsync();

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

        public async Task<IEnumerable<GetAllProductsViewModel>> GetProductsForHomeIndexAsync()
        {
            List<Product> products = await this.productRepository
            .GetAllAttached()
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .Take(4)
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
    }
}
