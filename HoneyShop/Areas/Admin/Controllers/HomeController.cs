namespace HoneyShop.Areas.Admin.Controllers
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.Home;
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    using Microsoft.AspNetCore.Mvc;
    public class HomeController : BaseAdminController
    {
        private readonly IOrderService orderService;
        private readonly IWarehouseViewComponentService warehouseService;
        private readonly IProductService productService;
        private readonly IUserService userService;

        public HomeController(
            IOrderService orderService,
            IWarehouseViewComponentService warehouseService,
            IProductService productService,
            IUserService userService)
        {
            this.orderService = orderService;
            this.warehouseService = warehouseService;
            this.productService = productService;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            DashboardOrderStats? orderStats = await orderService.GetOrderStatisticsAsync();

            IEnumerable<RecentOrderViewModel>? recentOrders = await orderService.GetRecentOrdersAsync(5);

            IEnumerable<BestSellingProductViewModel>? bestSellingProducts = await productService.GetBestSellingProductsAsync(5);

            IEnumerable<GetAllWarehouseViewModel>? warehouses = await warehouseService.GetAllWarehousesAsync();
            IEnumerable<WarehouseStockSummaryViewModel>? warehouseSummaries = warehouses.Select(w => new WarehouseStockSummaryViewModel
            {
                Id = w.Id,
                Name = w.Name,
            }).ToList();

            ProductStatisticsViewModel? productStats = await productService.GetProductStatisticsAsync();

            CustomerStatisticsViewModel? customerStats = await userService.GetCustomerStatisticsAsync();

            DashboardViewModel? viewModel = new DashboardViewModel
            {
                // Order statistics
                TotalOrders = orderStats.TotalOrders,
                PendingOrders = orderStats.PendingOrders,
                ConfirmedOrders = orderStats.ConfirmedOrders,
                ShippedOrders = orderStats.SentOrders,
                CompletedOrders = orderStats.FinishedOrders,

                // Sales statistics
                TotalSales = orderStats.TotalSales,
                MonthlySales = orderStats.MonthlySales,
                WeeklySales = orderStats.WeeklySales,
                DailySales = orderStats.DailySales,

                // Inventory statistics
                TotalProducts = productStats.TotalProducts,
                LowStockProducts = productStats.LowStockProducts,
                OutOfStockProducts = productStats.OutOfStockProducts,

                // Customer statistics
                TotalCustomers = customerStats.TotalCustomers,
                NewCustomersThisMonth = customerStats.NewCustomersThisMonth,

                // Recent activity
                RecentOrders = recentOrders,
                BestSellingProducts = bestSellingProducts,

                // Warehouse statistics
                TotalWarehouses = warehouseSummaries.Count(),
                WarehouseSummaries = warehouseSummaries
            };

            return View(viewModel);
        }
    }
}
