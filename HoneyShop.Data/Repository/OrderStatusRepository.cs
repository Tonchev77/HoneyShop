namespace HoneyShop.Data.Repository
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    public class OrderStatusRepository : BaseRepository<OrderStatus, Guid>, IOrderStatusRepository
    {
        public OrderStatusRepository(HoneyShopDbContext dbContext) 
            : base(dbContext)
        {

        }
    }
}
