namespace HoneyShop.Services.Core.Admin.Contracts
{
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    public interface IWarehouseService
    {
        Task<IEnumerable<WarehouseManagmentIndexViewModel>> GetAllWarehousesAsync();

        Task<bool> AddWarehouseAsync(AddWarehouseViewModel inputModel);
    }
}
