namespace HoneyShop.Services.Core.Contracts
{
    using HoneyShop.ViewModels.Shop;
    public interface IProductService
    {
        Task<IEnumerable<GetAllProductsViewModel>> GetAllProductsAsync();
    }
}
