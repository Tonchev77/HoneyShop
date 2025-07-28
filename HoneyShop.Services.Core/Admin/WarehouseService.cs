namespace HoneyShop.Services.Core.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            this.warehouseRepository = warehouseRepository;
        }

        public async Task<bool> AddWarehouseAsync(AddWarehouseViewModel inputModel)
        {
            bool opResult = false;

            if (inputModel != null)
            {
                Warehouse newWarehouse = new Warehouse()
                {
                    Name = inputModel.Name,
                    Location = inputModel.Location,
                };

                await this.warehouseRepository.AddAsync(newWarehouse);
                await this.warehouseRepository.SaveChangesAsync();

                opResult = true;
            }

            return opResult;
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

        public async Task<DeleteWarehouseManagmentViewModel?> GetWarehouseForDeleteAsync(Guid? warehouseId)
        {
            DeleteWarehouseManagmentViewModel? deleteModel = null;

            if (warehouseId != null)
            {
                Warehouse? deleteWarehouseModel = await this.warehouseRepository
                    .SingleOrDefaultAsync(c => c.Id == warehouseId);

                if (deleteWarehouseModel != null)
                {
                    deleteModel = new DeleteWarehouseManagmentViewModel()
                    {
                        Id = deleteWarehouseModel.Id,
                        Name = deleteWarehouseModel.Name,
                        Location = deleteWarehouseModel.Location
                    };
                }
            }

            return deleteModel;
        }

        public async Task<EditWarehouseManagmentViewModel?> GetWarehouseForEditingAsync(Guid? warehouseId)
        {
            EditWarehouseManagmentViewModel? editModel = null;

            if (warehouseId != null)
            {
                Warehouse? editWarehouseModel = await this.warehouseRepository
                    .GetAllAttached()
                    .SingleOrDefaultAsync(c => c.Id == warehouseId);

                if (editWarehouseModel != null)
                {
                    editModel = new EditWarehouseManagmentViewModel()
                    {
                        Id = editWarehouseModel.Id,
                        Name = editWarehouseModel.Name,
                        Location = editWarehouseModel.Location
                    };
                }
            }

            return editModel;
        }

        public async Task<bool> PersistUpdateWarehouseAsync(EditWarehouseManagmentViewModel inputModel)
        {
            bool opResult = false;

            Warehouse? updatedWarehouse = await this.warehouseRepository
                .FirstOrDefaultAsync(c => c.Id == inputModel.Id);


            if (updatedWarehouse != null)
            {
                updatedWarehouse.Id = inputModel.Id;
                updatedWarehouse.Name = inputModel.Name;
                updatedWarehouse.Location = inputModel.Location;


                await this.warehouseRepository.SaveChangesAsync();

                opResult = true;
            }

            return opResult;
        }

        public async Task<bool> SoftDeleteWarehouseAsync(DeleteWarehouseManagmentViewModel inputModel)
        {
            bool opResult = false;

            Warehouse? deletedWarehouse = await this.warehouseRepository
                .FirstOrDefaultAsync(c => c.Id == inputModel.Id);

            if (deletedWarehouse != null)
            {
                deletedWarehouse.IsDeleted = true;

                await this.warehouseRepository.SaveChangesAsync();

                opResult = true;
            }

            return opResult;
        }
    }
}
