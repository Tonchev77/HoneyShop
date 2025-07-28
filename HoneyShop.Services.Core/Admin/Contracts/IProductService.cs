namespace HoneyShop.Services.Core.Admin.Contracts
{
    using HoneyShop.ViewModels.Admin.ProductManagment;
    public interface IProductService
    {
        Task<IEnumerable<ProductManagmentIndexViewModel>> GetAllProductsAsync();

        Task<bool> AddProductAsync(string userId, AddProductViewModel inputModel);

        Task<EditProductManagmentViewModel?> GetProductForEditingAsync(Guid? productId);

        Task<bool> PersistUpdateProductAsync(EditProductManagmentViewModel inputModel);

        Task<DeleteProductManagmentViewModel?> GetProductForDeleteAsync(Guid? productId);

        Task<bool> SoftDeleteProductAsync(DeleteProductManagmentViewModel inputModel);
    }
}
