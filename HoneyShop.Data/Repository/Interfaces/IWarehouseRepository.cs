namespace HoneyShop.Data.Repository.Interfaces
{
    using HoneyShop.Data.Models;
    public interface IWarehouseRepository : IRepository<Warehouse, Guid>, IAsyncRepository<Warehouse, Guid>
    {

    }
}
