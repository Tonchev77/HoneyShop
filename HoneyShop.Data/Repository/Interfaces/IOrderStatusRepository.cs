namespace HoneyShop.Data.Repository.Interfaces
{
    using HoneyShop.Data.Models;
    public interface IOrderStatusRepository : IRepository<OrderStatus, Guid>, IAsyncRepository<OrderStatus, Guid>
    {

    }
}
