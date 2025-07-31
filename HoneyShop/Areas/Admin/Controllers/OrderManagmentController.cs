namespace HoneyShop.Areas.Admin.Controllers
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.OrderManagment;
    using Microsoft.AspNetCore.Mvc;
    public class OrderManagmentController : BaseAdminController
    {
        private readonly IOrderService orderService;

        public OrderManagmentController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<OrderViewModel> orders = await orderService.GetAllOrdersAsync();
            return View(orders);
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            OrderDetailsViewModel? orderDetails = await orderService.GetOrderDetailsAsync(id);

            if (orderDetails == null)
            {
                return NotFound();
            }

            ViewData["Statuses"] = await orderService.GetAllOrderStatusesAsync();

            return View(orderDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateOrderStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input. Please try again.";
                return RedirectToAction(nameof(Details), new { id = model.OrderId });
            }

            bool success = await orderService.UpdateOrderStatusAsync(model.OrderId, model.StatusId);

            if (success)
            {
                TempData["SuccessMessage"] = "Order status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update order status.";
            }

            return RedirectToAction(nameof(Details), new { id = model.OrderId });
        }
    }
}
