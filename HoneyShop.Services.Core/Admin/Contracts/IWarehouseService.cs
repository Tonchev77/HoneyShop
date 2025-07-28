using HoneyShop.ViewModels.Admin.WarehouseManagment;

namespace HoneyShop.Services.Core.Admin.Contracts
{
    public interface IWarehouseService
    {
        Task<IEnumerable<WarehouseManagmentIndexViewModel>> GetAllWarehousesAsync();
    }
}
