namespace HoneyShop.Areas.Admin.Controllers
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    using Microsoft.AspNetCore.Mvc;
    public class HomeController : BaseAdminController
    {
        private readonly IWarehouseViewComponentService warehouseService;
        public HomeController(IWarehouseViewComponentService warehouseService)
        {
            this.warehouseService = warehouseService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<GetAllWarehouseViewModel> warehouses = await this.warehouseService.GetAllWarehousesAsync();

            return View(warehouses);
        }

    }
}
