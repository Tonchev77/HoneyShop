namespace HoneyShop.Services.Core.Admin.Contracts
{
    using HoneyShop.ViewModels.Admin.CategoryManagment;
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryManagmentIndexViewModel>> GetAllCategoriesAsync();

        Task<bool> AddCategoryAsync(AddCategoryViewModel inputModel);

        Task<EditCategoryManagmentViewModel?> GetCategoryForEditingAsync(Guid? categoryId);

        Task<bool> PersistUpdateCategoryAsync(EditCategoryManagmentViewModel inputModel);
    }
}
