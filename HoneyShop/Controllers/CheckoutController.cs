namespace HoneyShop.Controllers
{
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Cart;
    using HoneyShop.ViewModels.Order;
    using Microsoft.AspNetCore.Mvc;

    public class CheckoutController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly ICartService cartService;

        public CheckoutController(IOrderService orderService, ICartService cartService)
        {
            this.orderService = orderService;
            this.cartService = cartService;
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

                IEnumerable<GetAllCartItemsViewModel> cartItems = await cartService.GetAllCartProductsAsync(userId);

                if (!cartItems.Any())
                {
                    return RedirectToAction("Index", "Cart");
                }

                decimal total = cartItems.Sum(ci => ci.ProductDetails.Price * ci.Quantity);

                CreateOrderViewModel model = new CreateOrderViewModel
                {
                    TotalAmount = total
                };

                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CreateOrderViewModel model)
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

                if (!ModelState.IsValid)
                {
                    IEnumerable<GetAllCartItemsViewModel> cartItems = await cartService.GetAllCartProductsAsync(userId);
                    model.TotalAmount = cartItems.Sum(ci => ci.ProductDetails.Price * ci.Quantity);

                    return View("Index", model);
                }

                Guid orderId = await orderService.CreateOrderAsync(userId, model);

                return RedirectToAction("Confirmation", new { id = orderId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while placing your order. Please try again.");

                try
                {
                    string? userId = GetUserId();
                    if (!string.IsNullOrEmpty(userId))
                    {
                        IEnumerable<GetAllCartItemsViewModel> cartItems = await cartService.GetAllCartProductsAsync(userId);
                        model.TotalAmount = cartItems.Sum(ci => ci.ProductDetails.Price * ci.Quantity);
                    }
                }
                catch
                {
                    // If recalculation fails, at least don't lose what we had
                }

                return View("Index", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(Guid id)
        {
            try
            {
                if (!IsUserAuthenticated())
                {
                    return RedirectToAction("Login", "Account");
                }

                OrderConfirmationViewModel? confirmation = await orderService.GetOrderConfirmationAsync(id);

                if (confirmation == null)
                {
                    return NotFound();
                }

                return View(confirmation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
