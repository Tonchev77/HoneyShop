namespace HoneyShop.Services.Core.Contracts
{
    using HoneyShop.ViewModels.Shop;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task<IEnumerable<GetAllProductsViewModel>> GetAllProductsAsync();
        Task<IEnumerable<GetAllProductsViewModel>> GetAllProductsByCategoryAsync(Guid? id);

        Task<IEnumerable<GetAllProductsViewModel>> GetAllProductsByStringAsync(string? searchString);

        Task<IEnumerable<GetAllProductsViewModel>> GetAllProductsByStringAndCategoryAsync(string searchString, Guid categoryId);
        Task<GetProductDetailViewModel?> GetProductDetailAsync(Guid? id);

        Task<IEnumerable<GetAllProductsViewModel>> GetProductsForHomeIndexAsync();
    }
}
