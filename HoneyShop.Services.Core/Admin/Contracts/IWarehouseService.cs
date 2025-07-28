namespace HoneyShop.Services.Core.Admin.Contracts
{
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    public interface IWarehouseService
    {
        Task<IEnumerable<WarehouseManagmentIndexViewModel>> GetAllWarehousesAsync();

        Task<bool> AddWarehouseAsync(AddWarehouseViewModel inputModel);

        Task<EditWarehouseManagmentViewModel?> GetWarehouseForEditingAsync(Guid? warehouseId);

        Task<bool> PersistUpdateWarehouseAsync(EditWarehouseManagmentViewModel inputModel);

        Task<DeleteWarehouseManagmentViewModel?> GetWarehouseForDeleteAsync(Guid? warehouseId);

        Task<bool> SoftDeleteWarehouseAsync(DeleteWarehouseManagmentViewModel inputModel);
    }
}
