namespace HoneyShop.Data.Repository.Interfaces
{
    using HoneyShop.Data.Models;
    public interface IOrderRepository : IRepository<Order, Guid>, IAsyncRepository<Order, Guid>
    {

    }
}
