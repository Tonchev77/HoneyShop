namespace HoneyShop.Areas.Admin.Controllers
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    using Microsoft.AspNetCore.Mvc;

    using static HoneyShop.GCommon.ApplicationConstants;
    using static HoneyShop.GCommon.NotificationMessages.Warehouse;
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

        [HttpGet]
        public async Task<IActionResult> AddWarehouse()
        {
            try
            {
                AddWarehouseViewModel inputModel = new AddWarehouseViewModel()
                {
                    Name = string.Empty,
                    Location = string.Empty
                };

                return this.View(inputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = WarehouseFatalError;
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddWarehouse(AddWarehouseViewModel inputModel)
        {
            try
            {
                bool addResult = await this.warehouseService
                    .AddWarehouseAsync(inputModel);

                if (addResult == false)
                {
                    TempData[ErrorMessageKey] = WarehouseEditError;
                    ModelState.AddModelError(string.Empty, WarehouseEditError);
                    return this.View(inputModel);
                }

                TempData[SuccessMessageKey] = WarehouseAddedSuccessfully;
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = WarehouseFatalError;
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
