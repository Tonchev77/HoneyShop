namespace HoneyShop.Data.Repository.Interfaces
{
    using HoneyShop.Data.Models;

    public interface IManagerRepository :
        IRepository<Manager, Guid>, IAsyncRepository<Manager, Guid>
    {
    }
}
