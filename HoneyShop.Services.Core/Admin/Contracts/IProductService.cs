namespace HoneyShop.Services.Core.Admin.Contracts
{
    using HoneyShop.ViewModels.Admin.ProductManagment;
    public interface IProductService
    {
        Task<IEnumerable<ProductManagmentIndexViewModel>> GetAllProductsAsync();

        Task<bool> AddProductAsync(string userId, AddProductViewModel inputModel);
    }
}
