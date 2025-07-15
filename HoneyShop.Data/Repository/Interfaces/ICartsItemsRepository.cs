namespace HoneyShop.Data.Repository.Interfaces
{
    using HoneyShop.Data.Models;

    public interface ICartsItemsRepository : IRepository<CartItem, object>, IAsyncRepository<CartItem, object>
    {
        CartItem? GetByCompositeKey(string userId, string productId);

        Task<CartItem?> GetByCompositeKeyAsync(string userId, string productId);

        bool Exists(string userId, string productId);

        Task<bool> ExistsAsync(string userId, string productId);
    }
}
