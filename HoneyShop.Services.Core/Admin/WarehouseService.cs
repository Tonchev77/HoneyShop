namespace HoneyShop.Services.Core.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            this.warehouseRepository = warehouseRepository;
        }

        public async Task<IEnumerable<WarehouseManagmentIndexViewModel>> GetAllWarehousesAsync()
        {
            List<Warehouse> warehouses = await this.warehouseRepository
            .GetAllAttached()
            .ToListAsync();

            IEnumerable<WarehouseManagmentIndexViewModel> allWarehouses = warehouses
                .Select(p => new WarehouseManagmentIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Location = p.Location
                })
                .ToList();

            return allWarehouses;
        }
    }
}
