namespace HoneyShop.Services.Core.Contracts
{
    using HoneyShop.ViewModels.Cart;
    public interface ICartService
    {
        Task<IEnumerable<GetAllCartItemsViewModel>> GetAllCartProductsAsync(string userId);

        Task<bool> AddProductToUserCartAsync(string userId, Guid productId);

        Task<bool> DeleteProductFromCartAsync(string userId, DeleteProductFromCartViewModel model);
    }
}

