namespace HoneyShop.Services.Core.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.CategoryManagment;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<bool> AddCategoryAsync(AddCategoryViewModel inputModel)
        {
            bool opResult = false;

            if (inputModel != null)
            {
                Category newCategory = new Category()
                {
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                };

                await this.categoryRepository.AddAsync(newCategory);
                await this.categoryRepository.SaveChangesAsync();

                opResult = true;
            }

            return opResult;
        }

        public async Task<DeleteCategoryManagmentViewModel?> GetCategoryForDeleteAsync(Guid? categoryId)
        {
            DeleteCategoryManagmentViewModel? deleteModel = null;

            if (categoryId != null)
            {
                Category? deleteCategoryModel = await this.categoryRepository
                    .SingleOrDefaultAsync(c => c.Id == categoryId);

                if (deleteCategoryModel != null)
                { 
                    deleteModel = new DeleteCategoryManagmentViewModel()
                    {
                        Id = deleteCategoryModel.Id,
                        Name = deleteCategoryModel.Name,
                        Description = deleteCategoryModel.Description ?? string.Empty
                    };
                }
            }

            return deleteModel;
        }

        public async Task<IEnumerable<CategoryManagmentIndexViewModel>> GetAllCategoriesAsync()
        {
            List<Category> categories = await this.categoryRepository
            .GetAllAttached()
            .ToListAsync();

            IEnumerable<CategoryManagmentIndexViewModel> allCategories = categories
                .Select(p => new CategoryManagmentIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description!
                })
                .ToList();

            return allCategories;

        }

        public async Task<EditCategoryManagmentViewModel?> GetCategoryForEditingAsync(Guid? categoryId)
        {
            EditCategoryManagmentViewModel? editModel = null;

            if (categoryId != null)
            {
                Category? editCategoryModel = await this.categoryRepository
                    .GetAllAttached()
                    .SingleOrDefaultAsync(c => c.Id == categoryId);

                if (editCategoryModel != null) 
                {
                    editModel = new EditCategoryManagmentViewModel()
                    {
                       Id = editCategoryModel.Id,
                       Name = editCategoryModel.Name,
                       Description = editCategoryModel.Description ?? string.Empty,
                    };
                }
            }

            return editModel;
        }

        public async Task<bool> PersistUpdateCategoryAsync(EditCategoryManagmentViewModel inputModel)
        {
            bool opResult = false;

            Category? updatedCategory = await this.categoryRepository
                .FirstOrDefaultAsync(c => c.Id == inputModel.Id);


            if (updatedCategory != null) 
            {
                updatedCategory.Id = inputModel.Id;
                updatedCategory.Name = inputModel.Name;
                updatedCategory.Description = inputModel.Description;


                await this.categoryRepository.SaveChangesAsync();

                opResult = true;
            }

            return opResult;
        }

        public async Task<bool> SoftDeleteCategoryAsync(DeleteCategoryManagmentViewModel inputModel)
        {
            bool opResult = false;

            Category? deletedCategory = await this.categoryRepository
                .FirstOrDefaultAsync(c => c.Id == inputModel.Id);

            if (deletedCategory != null)
            {
                deletedCategory.IsDeleted = true;

                await this.categoryRepository.SaveChangesAsync();

                opResult = true;
            }

            return opResult;
        }

        public async Task<IEnumerable<AddProductCategoryDropDownModel>> GetCategoryDropdownDataAsync()
        {
            IEnumerable<Category> categories = await this.categoryRepository
                .GetAllAttached()
                .ToListAsync();

            IEnumerable<AddProductCategoryDropDownModel> dropdownModels = categories
                .Select(c => new AddProductCategoryDropDownModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            return dropdownModels;
        }
    }
}
