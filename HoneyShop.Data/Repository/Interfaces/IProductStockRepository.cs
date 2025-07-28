namespace HoneyShop.Data.Repository.Interfaces
{
    using HoneyShop.Data.Models;
    public interface IProductStockRepository : IRepository<ProductStock, Guid>, IAsyncRepository<ProductStock, Guid>
    {

    }
}
