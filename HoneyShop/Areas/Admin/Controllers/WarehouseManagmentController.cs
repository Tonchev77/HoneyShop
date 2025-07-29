namespace HoneyShop.Areas.Admin.Controllers
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    using Microsoft.AspNetCore.Mvc;

    using static HoneyShop.GCommon.ApplicationConstants;
    using static HoneyShop.GCommon.NotificationMessages.Warehouse;
    using static HoneyShop.GCommon.NotificationMessages.ProductStock;
    public class WarehouseManagmentController : BaseAdminController
    {
        private readonly IWarehouseService warehouseService;
        private readonly IProductService productService;

        public WarehouseManagmentController(IWarehouseService warehouseService, IProductService productService)
        {
            this.warehouseService = warehouseService;
            this.productService = productService;
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
        public IActionResult AddWarehouse()
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

        [HttpGet]
        public async Task<IActionResult> EditWarehouse(Guid? warehouseId)
        {
            try
            {

                EditWarehouseManagmentViewModel? editInputModel = await this.warehouseService
                    .GetWarehouseForEditingAsync(warehouseId);

                if (editInputModel == null)
                {
                    TempData[ErrorMessageKey] = WarehouseNotFound;
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(editInputModel);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = WarehouseFatalError;
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditWarehouse(EditWarehouseManagmentViewModel inputModel)
        {
            try
            {
                bool editResult = false;

                if (!this.ModelState.IsValid)
                {
                    return this.View(inputModel);
                }

                editResult = await this.warehouseService.PersistUpdateWarehouseAsync(inputModel);
                if (editResult == false)
                {

                    TempData[ErrorMessageKey] = WarehouseEditError;
                    this.ModelState.AddModelError(string.Empty, WarehouseEditError);
                    return this.View(inputModel);
                }
                TempData[SuccessMessageKey] = WarehouseEditedSuccessfully;
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = WarehouseFatalError;
                return this.RedirectToAction(nameof(Index));

            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteWarehouse(Guid? warehouseId)
        {
            try
            {
                DeleteWarehouseManagmentViewModel? deleteInputModel = await this.warehouseService
                    .GetWarehouseForDeleteAsync(warehouseId);

                if (deleteInputModel == null)
                {
                    TempData[ErrorMessageKey] = WarehouseNotFound;
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(deleteInputModel);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = WarehouseFatalError;
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteWarehouse(DeleteWarehouseManagmentViewModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    TempData[ErrorMessageKey] = WarehouseDeletedError;
                    ModelState.AddModelError(string.Empty, WarehouseDeletedError);

                    return this.View(inputModel);
                }

                bool deleteResult = await this.warehouseService.SoftDeleteWarehouseAsync(inputModel);

                if (deleteResult == false)
                {
                    TempData[ErrorMessageKey] = WarehouseDeletedError;
                    this.ModelState.AddModelError(string.Empty, WarehouseDeletedError);

                    return this.View(inputModel);
                }

                TempData[SuccessMessageKey] = WarehouseDeletedSuccessfully;
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = WarehouseFatalError;

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid warehouseId)
        {

            try
            {
                ViewData["WarehouseId"] = warehouseId;

                IEnumerable<GetProductsInWarehouseViewModel> products = 
                    await warehouseService.GetProductsInWarehouseAsync(warehouseId);

                return View(products);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }         
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct(Guid warehouseId)
        {
            try
            {
                var products = await productService.GetAllProductsAsync();

                AddProductToWarehouseViewModel model = new AddProductToWarehouseViewModel
                {
                    WarehouseId = warehouseId,
                    Products = products
                };

                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = ProductStockFatalError;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductToWarehouseViewModel inputModel)
        {
            try
            {
                ModelState.Remove("Products");

                if (!ModelState.IsValid)
                {
                    inputModel.Products = await productService.GetAllProductsAsync();
                    return View(inputModel);
                }

                bool addResult = await warehouseService.AddProductToWarehouseAsync(inputModel);

                if (!addResult)
                {
                    inputModel.Products = await productService.GetAllProductsAsync();

                    ModelState.AddModelError(string.Empty, ProductStockFatalError);
                    TempData[ErrorMessageKey] = ProductStockFatalError;
                    return View(inputModel);
                }

                TempData[SuccessMessageKey] = ProductStockAddedSuccessfully;
                return RedirectToAction(nameof(Details), new { warehouseId = inputModel.WarehouseId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = ProductStockFatalError;

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid warehouseId, Guid productId) 
        {
            try
            {
                EditProductFromWarehouseViewModel? editInputModel = await this.warehouseService
                    .GetProductFromWarehouseForEditingAsync(warehouseId, productId);

                if (editInputModel == null)
                {
                    TempData[ErrorMessageKey] = ProductStockNotFound;
                    return this.RedirectToAction(nameof(Details), new { warehouseId });
                }

                return this.View(editInputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = ProductStockFatalError;
                return this.RedirectToAction(nameof(Details), new { warehouseId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductFromWarehouseViewModel inputModel)
        {
            try
            {
                ModelState.Remove("Products");

                if (!this.ModelState.IsValid)
                {
                    inputModel.Products = await productService.GetAllProductsAsync();
                    return this.View(inputModel);
                }

                bool updateResult = await warehouseService.PersistUpdateProductFromWarehouseAsync(inputModel);

                if (!updateResult)
                {
                    inputModel.Products = await productService.GetAllProductsAsync();
                    ModelState.AddModelError(string.Empty, ProductStockEditError);
                    TempData[ErrorMessageKey] = ProductStockEditError;

                    return this.View(inputModel);
                }

                TempData[SuccessMessageKey] = ProductStockEditedSuccessfully;
                return RedirectToAction(nameof(Details), new { warehouseId = inputModel.WarehouseId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = ProductStockFatalError;
                return this.RedirectToAction(nameof(Details), new { warehouseId = inputModel.WarehouseId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(Guid warehouseId, Guid productId)
        {
            try
            {
                DeleteProductFromWarehouseViewModel? deleteInputModel = await this.warehouseService
                    .GetProductFromWarehouseForDeleteAsync(warehouseId, productId);

                if (deleteInputModel == null)
                {
                    TempData[ErrorMessageKey] = ProductStockNotFound;
                    return this.RedirectToAction(nameof(Details), new { warehouseId });
                }

                return this.View(deleteInputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = ProductStockFatalError;
                return this.RedirectToAction(nameof(Details), new { warehouseId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteProduct(DeleteProductFromWarehouseViewModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    TempData[ErrorMessageKey] = ProductStockDeletedError;
                    ModelState.AddModelError(string.Empty, ProductStockDeletedError);

                    return this.View(inputModel);
                }

                bool deleteResult = await warehouseService.SoftDeleteProductFromWarehouseAsync(inputModel);

                if (!deleteResult)
                {
                    TempData[ErrorMessageKey] = ProductStockDeletedError;
                    ModelState.AddModelError(string.Empty, ProductStockDeletedError);

                    return this.View(inputModel);
                }

                TempData[SuccessMessageKey] = ProductStockDeletedSuccessfully;
                return RedirectToAction(nameof(Details), new { warehouseId = inputModel.WarehouseId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = ProductStockFatalError;

                return RedirectToAction(nameof(Details), new { warehouseId = inputModel.WarehouseId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RecoverProduct(Guid warehouseId, Guid productId)
        {
            try
            {
                bool recoverResult = await warehouseService.RecoverProductFromWarehouseAsync(warehouseId, productId);

                if (!recoverResult)
                {
                    TempData[ErrorMessageKey] = ProductStockRecoverError;
                    return RedirectToAction(nameof(Details), new { warehouseId });
                }

                TempData[SuccessMessageKey] = ProductStockRecoverSuccessfully;
                return RedirectToAction(nameof(Details), new { warehouseId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData[ErrorMessageKey] = ProductStockFatalError;
                return RedirectToAction(nameof(Details), new { warehouseId });
            }
        }
    }
}
