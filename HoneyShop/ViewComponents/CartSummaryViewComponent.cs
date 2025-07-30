namespace HoneyShop.ViewComponents
{
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Cart;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartSummaryViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return View("Empty");
            }

            string? userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<GetAllCartItemsViewModel> cartItems = await _cartService.GetAllCartProductsAsync(userId!);

            return View(cartItems);
        }
    }
}
