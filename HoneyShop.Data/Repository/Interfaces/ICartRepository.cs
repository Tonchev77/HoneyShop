namespace HoneyShop.Data.Repository.Interfaces
{
    using HoneyShop.Data.Models;
    public interface ICartRepository : IRepository<Cart, Guid>, IAsyncRepository<Cart, Guid>
    {

    }
}
