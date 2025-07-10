namespace HoneyShop.Data.Repository.Interfaces
{
    using HoneyShop.Data.Models;
    public interface IProductRepository 
        : IRepository<Product, Guid>, IAsyncRepository<Product, Guid>
    {
    }
}
