namespace HoneyShop.Controllers
{
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Order;
    using Microsoft.AspNetCore.Mvc;
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                if (!IsUserAuthenticated())
                {
                    return RedirectToAction("Login", "Account");
                }

                string? userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                IEnumerable<GetAllOrdersForUserViewModel> orders = await orderService.GetUserOrdersAsync(userId);

                return View(orders);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
