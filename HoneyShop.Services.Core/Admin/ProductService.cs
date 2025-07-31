namespace HoneyShop.Services.Core.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.CategoryManagment;
    using HoneyShop.ViewModels.Admin.Home;
    using HoneyShop.ViewModels.Admin.ProductManagment;
    using HoneyShop.ViewModels.Home;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, 
            UserManager<ApplicationUser> userManager, IOrderItemRepository orderItemRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.userManager = userManager;
            this.orderItemRepository = orderItemRepository;
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

        public async Task<DeleteProductManagmentViewModel?> GetProductForDeleteAsync(Guid? productId)
        {
            DeleteProductManagmentViewModel? deleteModel = null;

            if (productId != null)
            {
                Product? deleteProductModel = await this.productRepository
                    .SingleOrDefaultAsync(c => c.Id == productId);

                if (deleteProductModel != null)
                {
                    deleteModel = new DeleteProductManagmentViewModel()
                    {
                        Id = deleteProductModel.Id,
                        Name = deleteProductModel.Name,
                        Description = deleteProductModel.Description ?? string.Empty,
                        Price = deleteProductModel.Price,
                        ImageUrl = deleteProductModel.ImageUrl,
                        IsActive = deleteProductModel.IsActive,
                    };
                }
            }

            return deleteModel;
        }

        public async Task<EditProductManagmentViewModel?> GetProductForEditingAsync(Guid? productId)
        {
            EditProductManagmentViewModel? editModel = null;

            if (productId != null)
            {
                Product? editProductModel = await this.productRepository
                    .SingleOrDefaultAsync(r => r.Id == productId);

                if (editProductModel != null)    
                {
                    editModel = new EditProductManagmentViewModel()
                    {
                        Id = editProductModel.Id,
                        Name = editProductModel.Name,
                        Description = editProductModel.Description,
                        Price = editProductModel.Price,
                        ImageUrl = editProductModel.ImageUrl,
                        IsActive = editProductModel.IsActive,
                        CategoryId = editProductModel.CategoryId,
                    };
                }
            }

            return editModel;
        }

        public async Task<bool> PersistUpdateProductAsync(EditProductManagmentViewModel inputModel)
        {
            bool opResult = false;

            Product? updatedProduct = await this.productRepository
                .FirstOrDefaultAsync(c => c.Id == inputModel.Id);


            if (updatedProduct != null)
            {
                updatedProduct.Id = inputModel.Id;
                updatedProduct.Name = inputModel.Name;
                updatedProduct.Description = inputModel.Description;
                updatedProduct.Price = inputModel.Price;
                updatedProduct.ImageUrl = inputModel.ImageUrl;
                updatedProduct.IsActive = inputModel.IsActive;
                updatedProduct.CategoryId = inputModel.CategoryId;


                await this.productRepository.SaveChangesAsync();

                opResult = true;
            }

            return opResult;
        }

        public async Task<bool> SoftDeleteProductAsync(DeleteProductManagmentViewModel inputModel)
        {
            bool opResult = false;

            Product? deletedProduct = await this.productRepository
                .FirstOrDefaultAsync(c => c.Id == inputModel.Id);

            if (deletedProduct != null)
            {
                deletedProduct.IsDeleted = true;

                await this.productRepository.SaveChangesAsync();

                opResult = true;
            }

            return opResult;
        }

        public async Task<ProductStatisticsViewModel> GetProductStatisticsAsync()
        {
            IEnumerable<Product> products = await productRepository
                .GetAllAttached()
                .Include(p => p.ProductStocks)
                .Where(p => !p.IsDeleted)
                .ToListAsync();

            var productsWithStock = products.Select(p => new
            {
                Product = p,
                TotalStock = p.ProductStocks.Where(ps => !ps.IsDeleted).Sum(ps => ps.Quantity)
            }).ToList();

            ProductStatisticsViewModel? stats = new ProductStatisticsViewModel
            {
                TotalProducts = products.Count(),

                LowStockProducts = productsWithStock.Count(p => p.TotalStock > 0 && p.TotalStock < 10),
 
                OutOfStockProducts = productsWithStock.Count(p => p.TotalStock == 0)
            };

            return stats;
        }

        public async Task<IEnumerable<BestSellingProductViewModel>> GetBestSellingProductsAsync(int count = 5)
        {
            var bestSellers = await orderItemRepository
                .GetAllAttached()
                .Where(oi => !oi.IsDeleted)
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi => !oi.Order.IsDeleted)
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Product = g.First().Product,
                    TotalSold = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(count)
                .ToListAsync();

            IEnumerable<BestSellingProductViewModel> bestSellingProducts = bestSellers.Select(bs => new BestSellingProductViewModel
            {
                Id = bs.ProductId,
                Name = bs.Product.Name,
                ImageUrl = bs.Product.ImageUrl,
                Price = bs.Product.Price,
                TotalSold = bs.TotalSold,
                TotalRevenue = bs.TotalRevenue
            }).ToList();

            return bestSellingProducts;
        }
    }
}
