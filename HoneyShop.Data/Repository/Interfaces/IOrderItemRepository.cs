namespace HoneyShop.Data.Repository.Interfaces
{
    using HoneyShop.Data.Models;
    public interface IOrderItemRepository : IRepository<OrderItem, Guid>, IAsyncRepository<OrderItem, Guid>
    {

    }
}
