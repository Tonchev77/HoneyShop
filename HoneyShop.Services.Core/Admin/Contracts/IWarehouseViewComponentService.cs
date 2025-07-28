namespace HoneyShop.Services.Core.Admin.Contracts
{
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    public interface IWarehouseViewComponentService
    {
        Task<IEnumerable<GetAllWarehouseViewModel>> GetAllWarehousesAsync();
    }
}
