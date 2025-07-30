namespace HoneyShop.Data.Repository
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    public class OrderItemRepository : BaseRepository<OrderItem, Guid>, IOrderItemRepository
    {
        public OrderItemRepository(HoneyShopDbContext context)
            : base(context)
        {

        }
    }
}
