namespace HoneyShop.Services.Core
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Cart;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartsItemsRepository cartsItemsRepository;
        private readonly IProductRepository productRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CartService(ICartRepository cartRepository, ICartsItemsRepository cartsItemsRepository, UserManager<ApplicationUser> userManager,
            IProductRepository productRepository)
        {
            this.cartRepository = cartRepository;
            this.cartsItemsRepository = cartsItemsRepository;
            this.userManager = userManager;
            this.productRepository = productRepository;
        }

        public async Task<bool> AddProductToUserCartAsync(string userId, Guid productId)
        {
            // Find user
            ApplicationUser? user = await this.userManager.FindByIdAsync(userId);

            // Find product
            Product? productToAdd = await this.productRepository
                .SingleOrDefaultAsync(p => p.Id == productId);

            if ((user != null) && (productToAdd != null))
            {
                // Find cart for user
                Cart? userCart = await this.cartRepository
                    .SingleOrDefaultAsync(c => c.UserId.ToLower() == userId.ToLower() && !c.IsDeleted);

                // If no cart, create one
                if (userCart == null)
                {
                    userCart = new Cart
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        IsDeleted = false,
                        DeletedAt = null
                    };
                    await this.cartRepository.AddAsync(userCart);
                    await this.cartRepository.SaveChangesAsync();
                }


                var allCartItems = await cartsItemsRepository.GetAllAttached()
                    .IgnoreQueryFilters()
                    .Where(ci => ci.CartId == userCart.Id && ci.ProductId == productId)
                    .ToListAsync();

                if (allCartItems.Any())
                {
                    // Item exists (active or deleted)
                    var cartItem = allCartItems.First();

                    if (cartItem.IsDeleted)
                    {
                        // Restore the deleted item
                        cartItem.IsDeleted = false;
                        cartItem.DeletedAt = null;
                        cartItem.Quantity = 1; // Reset quantity
                    }
                    else
                    {
                        // Increment quantity for active item
                        cartItem.Quantity += 1;
                    }

                    // Use Update instead of Add
                    cartsItemsRepository.Update(cartItem);
                    await cartsItemsRepository.SaveChangesAsync();

                    return true;
                }
                else
                {
                    // Item doesn't exist at all, create a new one
                    var newCartItem = new CartItem
                    {
                        CartId = userCart.Id,
                        ProductId = productId,
                        Quantity = 1,
                        IsDeleted = false,
                        DeletedAt = null
                    };

                    await cartsItemsRepository.AddAsync(newCartItem);
                    await cartsItemsRepository.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        public async Task<bool> DeleteProductFromCartAsync(string userId, DeleteProductFromCartViewModel model)
        {
            // Find user's cart
            Cart? userCart = await this.cartRepository
                .SingleOrDefaultAsync(c => c.UserId.ToLower() == userId.ToLower() && !c.IsDeleted);

            if (userCart == null || userCart.Id != model.CartId)
            {
                return false;
            }

            // Find the cart item to delete
            CartItem? cartItem = await this.cartsItemsRepository
                .SingleOrDefaultAsync(ci => ci.CartId == model.CartId &&
                                            ci.ProductId == model.ProductId &&
                                            !ci.IsDeleted);

            if (cartItem == null)
            {
                return false;
            }

            // Mark as deleted
            cartItem.IsDeleted = true;
            cartItem.DeletedAt = DateTime.UtcNow;

            this.cartsItemsRepository.Update(cartItem);
            await this.cartsItemsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<GetAllCartItemsViewModel>> GetAllCartProductsAsync(string userId)
        {
            Cart? cart = await this.cartRepository
                .SingleOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);

            if (cart == null)
            {
                return Enumerable.Empty<GetAllCartItemsViewModel>();
            }
                

            IEnumerable<CartItem> cartItems = await this.cartsItemsRepository
                .GetAllAttached()
                .Where(ci => ci.CartId == cart.Id && !ci.IsDeleted)
                .Include(ci => ci.Product)
                .ToListAsync();

            IEnumerable<GetAllCartItemsViewModel> result = cartItems.Select(ci => new GetAllCartItemsViewModel
            {
                CartId = ci.CartId,
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                ProductDetails = new GetProductDetailsForCartViewModel
                {
                    Id = ci.Product.Id,
                    ImageUrl = ci.Product.ImageUrl,
                    Name = ci.Product.Name,
                    Price = ci.Product.Price
                }
            });

            return result;
        }
    }
}
