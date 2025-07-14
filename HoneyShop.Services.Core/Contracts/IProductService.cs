namespace HoneyShop.Services.Core.Contracts
{
    using HoneyShop.ViewModels.Shop;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task<IEnumerable<GetAllProductsViewModel>> GetAllProductsAsync();

        Task<GetProductDetailViewModel?> GetProductDetailAsync(Guid? id);
    }
}
