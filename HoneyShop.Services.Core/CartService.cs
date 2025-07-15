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
        private readonly UserManager<IdentityUser> userManager;

        public CartService(ICartRepository cartRepository, ICartsItemsRepository cartsItemsRepository, UserManager<IdentityUser> userManager,
            IProductRepository productRepository)
        {
            this.cartRepository = cartRepository;
            this.cartsItemsRepository = cartsItemsRepository;
            this.userManager = userManager;
            this.productRepository = productRepository;
        }

        public async Task<bool> AddProductToUserCartAsync(string userId, Guid productId)
        {
            bool opResult = false;

            // Find user
            IdentityUser? user = await this.userManager.FindByIdAsync(userId);

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

                // Check for existing cart item
                CartItem? userCartItem = await cartsItemsRepository
                    .SingleOrDefaultAsync(ur => ur.CartId == userCart.Id && ur.ProductId == productId && !ur.IsDeleted);

                if (userCartItem == null)
                {
                    userCartItem = new CartItem()
                    {
                        CartId = userCart.Id,
                        ProductId = productId,
                        Quantity = 1,
                        IsDeleted = false,
                        DeletedAt = null
                    };
                    await cartsItemsRepository.AddAsync(userCartItem);
                    opResult = true;
                }
                else
                {
                    // Increment quantity if already exists
                    userCartItem.Quantity += 1;
                    cartsItemsRepository.Update(userCartItem);
                    opResult = true;
                }

                await cartsItemsRepository.SaveChangesAsync();
            }

            return opResult;
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
