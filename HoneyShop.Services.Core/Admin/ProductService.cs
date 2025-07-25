namespace HoneyShop.Services.Core.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.CategoryManagment;
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
    }
}
