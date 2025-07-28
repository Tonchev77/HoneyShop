namespace HoneyShop.Data.Repository
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class ProductStockRepository : BaseRepository<ProductStock, Guid>, IProductStockRepository
    {
        public ProductStockRepository(HoneyShopDbContext dbContext) 
            : base(dbContext)
        {

        }
        
    }
}
