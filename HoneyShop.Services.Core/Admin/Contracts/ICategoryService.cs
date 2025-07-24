namespace HoneyShop.Services.Core.Admin.Contracts
{
    using HoneyShop.ViewModels.Admin.CategoryManagment;
    using HoneyShop.ViewModels.Shop;
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryManagmentIndexViewModel>> GetAllCategoriesAsync();
    }
}
