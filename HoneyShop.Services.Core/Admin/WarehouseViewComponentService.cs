namespace HoneyShop.Services.Core.Admin
{
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    using Microsoft.EntityFrameworkCore;

    public class WarehouseViewComponentService : IWarehouseViewComponentService
    {
        private readonly IWarehouseRepository warehouseRepository;
        public WarehouseViewComponentService(IWarehouseRepository warehouseRepository)
        {
            this.warehouseRepository = warehouseRepository;
        }
        public async Task<IEnumerable<GetAllWarehouseViewModel>> GetAllWarehousesAsync()
        {
            IEnumerable<GetAllWarehouseViewModel> warehouses = new List<GetAllWarehouseViewModel>();

            warehouses = await warehouseRepository
            .GetAllAttached()
            .Where(w => !w.IsDeleted)
            .Select(w => new GetAllWarehouseViewModel
            {
               Id = w.Id,
               Name = w.Name
             })
            .ToListAsync();

           return warehouses;
        }
    }
}
