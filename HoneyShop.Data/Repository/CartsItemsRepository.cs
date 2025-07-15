namespace HoneyShop.Data.Repository
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using Microsoft.EntityFrameworkCore;
    public class CartsItemsRepository : BaseRepository<CartItem, object>, ICartsItemsRepository
    {
        public CartsItemsRepository(HoneyShopDbContext dbContext) 
            : base(dbContext)
        {

        }

        public bool Exists(string userId, string productId)
        {
            return this
                .GetAllAttached()
                .Any(aum => aum.Cart.UserId.ToLower() == userId.ToLower() &&
                            aum.ProductId.ToString().ToLower() == productId.ToLower());
        }

        public Task<bool> ExistsAsync(string userId, string productId)
        {
            return this
                .GetAllAttached()
                .AnyAsync(aum => aum.Cart.UserId.ToLower() == userId.ToLower() &&
                            aum.ProductId.ToString().ToLower() == productId.ToLower());
        }

        public CartItem? GetByCompositeKey(string userId, string productId)
        {
            return this
                .GetAllAttached()
                .SingleOrDefault(ci => ci.Cart.UserId.ToLower() == userId.ToLower() &&
                        ci.ProductId.ToString().ToLower() == productId.ToLower());
        }

        public Task<CartItem?> GetByCompositeKeyAsync(string userId, string productId)
        {
            return this
                .GetAllAttached()
                .SingleOrDefaultAsync(ci => ci.Cart.UserId.ToLower() == userId.ToLower() &&
                        ci.ProductId.ToString().ToLower() == productId.ToLower());
        }
    }
}
