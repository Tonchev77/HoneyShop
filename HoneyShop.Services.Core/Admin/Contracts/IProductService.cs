namespace HoneyShop.Services.Core.Admin.Contracts
{
    using HoneyShop.ViewModels.Admin.CategoryManagment;
    using HoneyShop.ViewModels.Admin.ProductManagment;
    public interface IProductService
    {
        Task<IEnumerable<ProductManagmentIndexViewModel>> GetAllProductsAsync();

        Task<bool> AddCategoryAsync(AddCategoryViewModel inputModel);
    }
}
