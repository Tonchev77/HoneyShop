namespace HoneyShop.Areas.Admin.ViewComponents
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    using Microsoft.AspNetCore.Mvc;
    public class WarehousesViewComponent : ViewComponent
    {
        private readonly IWarehouseViewComponentService warehouseService;

        public WarehousesViewComponent(IWarehouseViewComponentService warehouseService)
        {
            this.warehouseService = warehouseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<GetAllWarehouseViewModel> warehouses = await warehouseService.GetAllWarehousesAsync();
            return View(warehouses);
        }
    }
}
