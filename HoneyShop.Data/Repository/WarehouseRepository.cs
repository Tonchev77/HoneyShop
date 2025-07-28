namespace HoneyShop.Data.Repository
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    public class WarehouseRepository : BaseRepository<Warehouse, Guid>, IWarehouseRepository
    {
        public WarehouseRepository(HoneyShopDbContext dbContext) 
            : base(dbContext)
        {

        }
    }
}
