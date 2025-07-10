namespace HoneyShop.Data.Repository.Interfaces
{
    using HoneyShop.Data.Models;
    public interface ICategoryRepository 
        : IRepository<Category, Guid>, IAsyncRepository<Category, Guid>
    {

    }
}
