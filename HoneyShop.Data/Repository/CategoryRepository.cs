namespace HoneyShop.Data.Repository
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    public class CategoryRepository : BaseRepository<Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(HoneyShopDbContext dbContext) 
            : base(dbContext)
        {

        }
    }
}
