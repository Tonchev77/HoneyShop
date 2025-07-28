namespace HoneyShop.Areas.Admin.Controllers
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    using Microsoft.AspNetCore.Mvc;
    public class WarehouseManagmentController : BaseAdminController
    {
        private readonly IWarehouseService warehouseService;

        public WarehouseManagmentController(IWarehouseService warehouseService)
        {
            this.warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<WarehouseManagmentIndexViewModel> allWarehouses = await
                    this.warehouseService.GetAllWarehousesAsync();

                return View(allWarehouses);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
