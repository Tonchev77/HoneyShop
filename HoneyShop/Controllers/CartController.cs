namespace HoneyShop.Controllers
{
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Cart;
    using Microsoft.AspNetCore.Mvc;

    public class CartController : BaseController
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

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

                return View(cartItems);
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid id)
        {
            try
            {
                if (!IsUserAuthenticated())
                {
                    return RedirectToAction("Login", "Account");
                }

                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                bool result = await cartService.AddProductToUserCartAsync(userId, id);

                if (result)
                {
                    // Product successfully added to cart, redirect to cart or product list
                    return RedirectToAction("Index", "Cart");
                }
                else
                {
                    // Could not add product, handle error (e.g. show a message or redirect)
                    return RedirectToAction(nameof(Index), "Shop");
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            } 
        }
    }
}
