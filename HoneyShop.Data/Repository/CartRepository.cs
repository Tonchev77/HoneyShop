namespace HoneyShop.Data.Repository
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    public class CartRepository : BaseRepository<Cart, Guid>, ICartRepository
    {
        public CartRepository(HoneyShopDbContext dbContext) 
            : base(dbContext)
        {

        }
    }
}
