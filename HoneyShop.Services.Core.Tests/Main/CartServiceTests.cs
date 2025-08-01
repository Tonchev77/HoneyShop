namespace HoneyShop.Services.Core.Tests.Main
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Cart;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using MockQueryable;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class CartServiceTests
    {
        private Mock<ICartRepository> cartRepositoryMock;
        private Mock<ICartsItemsRepository> cartsItemsRepositoryMock;
        private Mock<IProductRepository> productRepositoryMock;
        private Mock<UserManager<ApplicationUser>> userManagerMock;
        private ICartService cartService;

        [SetUp]
        public void Setup()
        {
            this.cartRepositoryMock = new Mock<ICartRepository>();
            this.cartsItemsRepositoryMock = new Mock<ICartsItemsRepository>();
            this.productRepositoryMock = new Mock<IProductRepository>();

            // Mock UserManager - this requires mocking the store and other dependencies
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            this.userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            this.cartService = new CartService(
                this.cartRepositoryMock.Object,
                this.cartsItemsRepositoryMock.Object,
                this.userManagerMock.Object,
                this.productRepositoryMock.Object);
        }

        #region AddProductToUserCartAsync Tests

        [Test]
        public async Task AddProductToUserCartAsync_UserNotFound_ShouldReturnFalse()
        {
            // Arrange
            string userId = "non-existent-user";
            Guid productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Test Product" };

            this.userManagerMock
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync((ApplicationUser?)null);

            this.productRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                .ReturnsAsync(product);

            // Act
            bool result = await this.cartService.AddProductToUserCartAsync(userId, productId);

            // Assert
            Assert.That(result, Is.False);
            this.userManagerMock.Verify(x => x.FindByIdAsync(userId), Times.Once);
            this.productRepositoryMock.Verify(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()), Times.Once);
        }

        [Test]
        public async Task AddProductToUserCartAsync_ProductNotFound_ShouldReturnFalse()
        {
            // Arrange
            string userId = "valid-user-id";
            Guid productId = Guid.NewGuid();
            var user = new ApplicationUser { Id = userId };

            this.userManagerMock
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            this.productRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                .ReturnsAsync((Product?)null);

            // Act
            bool result = await this.cartService.AddProductToUserCartAsync(userId, productId);

            // Assert
            Assert.That(result, Is.False);
            this.userManagerMock.Verify(x => x.FindByIdAsync(userId), Times.Once);
            this.productRepositoryMock.Verify(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()), Times.Once);
        }

        [Test]
        public async Task AddProductToUserCartAsync_NoExistingCart_ShouldCreateCartAndAddProduct()
        {
            // Arrange
            string userId = "valid-user-id";
            Guid productId = Guid.NewGuid();
            var user = new ApplicationUser { Id = userId };
            var product = new Product { Id = productId, Name = "Test Product" };

            this.userManagerMock
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            this.productRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                .ReturnsAsync(product);

            this.cartRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
                .ReturnsAsync((Cart?)null);

            var emptyCartItems = new List<CartItem>().BuildMock();
            this.cartsItemsRepositoryMock
                .Setup(x => x.GetAllAttached())
                .Returns(emptyCartItems);

            // Act
            bool result = await this.cartService.AddProductToUserCartAsync(userId, productId);

            // Assert
            Assert.That(result, Is.True);
            this.cartRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Once);
            this.cartRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            this.cartsItemsRepositoryMock.Verify(x => x.AddAsync(It.IsAny<CartItem>()), Times.Once);
            this.cartsItemsRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task AddProductToUserCartAsync_ExistingCartNoExistingItem_ShouldAddNewItem()
        {
            // Arrange
            string userId = "valid-user-id";
            Guid productId = Guid.NewGuid();
            Guid cartId = Guid.NewGuid();
            var user = new ApplicationUser { Id = userId };
            var product = new Product { Id = productId, Name = "Test Product" };
            var existingCart = new Cart { Id = cartId, UserId = userId };

            this.userManagerMock
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            this.productRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                .ReturnsAsync(product);

            this.cartRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
                .ReturnsAsync(existingCart);

            var emptyCartItems = new List<CartItem>().BuildMock();
            this.cartsItemsRepositoryMock
                .Setup(x => x.GetAllAttached())
                .Returns(emptyCartItems);

            // Act
            bool result = await this.cartService.AddProductToUserCartAsync(userId, productId);

            // Assert
            Assert.That(result, Is.True);
            this.cartRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Never);
            this.cartsItemsRepositoryMock.Verify(x => x.AddAsync(It.IsAny<CartItem>()), Times.Once);
            this.cartsItemsRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task AddProductToUserCartAsync_ExistingActiveItem_ShouldIncrementQuantity()
        {
            // Arrange
            string userId = "valid-user-id";
            Guid productId = Guid.NewGuid();
            Guid cartId = Guid.NewGuid();
            var user = new ApplicationUser { Id = userId };
            var product = new Product { Id = productId, Name = "Test Product" };
            var existingCart = new Cart { Id = cartId, UserId = userId };
            var existingCartItem = new CartItem 
            { 
                CartId = cartId, 
                ProductId = productId, 
                Quantity = 2, 
                IsDeleted = false 
            };

            this.userManagerMock
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            this.productRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                .ReturnsAsync(product);

            this.cartRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
                .ReturnsAsync(existingCart);

            var cartItemsList = new List<CartItem> { existingCartItem }.BuildMock();
            this.cartsItemsRepositoryMock
                .Setup(x => x.GetAllAttached())
                .Returns(cartItemsList);

            // Act
            bool result = await this.cartService.AddProductToUserCartAsync(userId, productId);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(existingCartItem.Quantity, Is.EqualTo(3));
            this.cartsItemsRepositoryMock.Verify(x => x.Update(existingCartItem), Times.Once);
            this.cartsItemsRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            this.cartsItemsRepositoryMock.Verify(x => x.AddAsync(It.IsAny<CartItem>()), Times.Never);
        }

        [Test]
        public async Task AddProductToUserCartAsync_ExistingDeletedItem_ShouldRestoreItem()
        {
            // Arrange
            string userId = "valid-user-id";
            Guid productId = Guid.NewGuid();
            Guid cartId = Guid.NewGuid();
            var user = new ApplicationUser { Id = userId };
            var product = new Product { Id = productId, Name = "Test Product" };
            var existingCart = new Cart { Id = cartId, UserId = userId };
            var deletedCartItem = new CartItem 
            { 
                CartId = cartId, 
                ProductId = productId, 
                Quantity = 5, 
                IsDeleted = true,
                DeletedAt = DateTime.UtcNow
            };

            this.userManagerMock
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            this.productRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>()))
                .ReturnsAsync(product);

            this.cartRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
                .ReturnsAsync(existingCart);

            var cartItemsList = new List<CartItem> { deletedCartItem }.BuildMock();
            this.cartsItemsRepositoryMock
                .Setup(x => x.GetAllAttached())
                .Returns(cartItemsList);

            // Act
            bool result = await this.cartService.AddProductToUserCartAsync(userId, productId);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(deletedCartItem.IsDeleted, Is.False);
            Assert.That(deletedCartItem.DeletedAt, Is.Null);
            Assert.That(deletedCartItem.Quantity, Is.EqualTo(1)); // Should reset to 1
            this.cartsItemsRepositoryMock.Verify(x => x.Update(deletedCartItem), Times.Once);
            this.cartsItemsRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region DeleteProductFromCartAsync Tests

        [Test]
        public async Task DeleteProductFromCartAsync_UserCartNotFound_ShouldReturnFalse()
        {
            // Arrange
            string userId = "valid-user-id";
            var model = new DeleteProductFromCartViewModel
            {
                CartId = Guid.NewGuid(),
                ProductId = Guid.NewGuid()
            };

            this.cartRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
                .ReturnsAsync((Cart?)null);

            // Act
            bool result = await this.cartService.DeleteProductFromCartAsync(userId, model);

            // Assert
            Assert.That(result, Is.False);
            this.cartRepositoryMock.Verify(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()), Times.Once);
            this.cartsItemsRepositoryMock.Verify(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<CartItem, bool>>>()), Times.Never);
        }

        [Test]
        public async Task DeleteProductFromCartAsync_CartIdMismatch_ShouldReturnFalse()
        {
            // Arrange
            string userId = "valid-user-id";
            Guid actualCartId = Guid.NewGuid();
            Guid requestedCartId = Guid.NewGuid();
            
            var model = new DeleteProductFromCartViewModel
            {
                CartId = requestedCartId,
                ProductId = Guid.NewGuid()
            };

            var userCart = new Cart { Id = actualCartId, UserId = userId };

            this.cartRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
                .ReturnsAsync(userCart);

            // Act
            bool result = await this.cartService.DeleteProductFromCartAsync(userId, model);

            // Assert
            Assert.That(result, Is.False);
            this.cartsItemsRepositoryMock.Verify(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<CartItem, bool>>>()), Times.Never);
        }

        [Test]
        public async Task DeleteProductFromCartAsync_CartItemNotFound_ShouldReturnFalse()
        {
            // Arrange
            string userId = "valid-user-id";
            Guid cartId = Guid.NewGuid();
            
            var model = new DeleteProductFromCartViewModel
            {
                CartId = cartId,
                ProductId = Guid.NewGuid()
            };

            var userCart = new Cart { Id = cartId, UserId = userId };

            this.cartRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
                .ReturnsAsync(userCart);

            this.cartsItemsRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<CartItem, bool>>>()))
                .ReturnsAsync((CartItem?)null);

            // Act
            bool result = await this.cartService.DeleteProductFromCartAsync(userId, model);

            // Assert
            Assert.That(result, Is.False);
            this.cartsItemsRepositoryMock.Verify(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<CartItem, bool>>>()), Times.Once);
            this.cartsItemsRepositoryMock.Verify(x => x.Update(It.IsAny<CartItem>()), Times.Never);
        }

        [Test]
        public async Task DeleteProductFromCartAsync_ValidCartItem_ShouldMarkAsDeleted()
        {
            // Arrange
            string userId = "valid-user-id";
            Guid cartId = Guid.NewGuid();
            Guid productId = Guid.NewGuid();
            
            var model = new DeleteProductFromCartViewModel
            {
                CartId = cartId,
                ProductId = productId
            };

            var userCart = new Cart { Id = cartId, UserId = userId };
            var cartItem = new CartItem 
            { 
                CartId = cartId, 
                ProductId = productId, 
                IsDeleted = false,
                Quantity = 2
            };

            this.cartRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
                .ReturnsAsync(userCart);

            this.cartsItemsRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<CartItem, bool>>>()))
                .ReturnsAsync(cartItem);

            // Act
            bool result = await this.cartService.DeleteProductFromCartAsync(userId, model);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(cartItem.IsDeleted, Is.True);
            Assert.That(cartItem.DeletedAt, Is.Not.Null);
            Assert.That(cartItem.DeletedAt.Value, Is.LessThan(DateTime.UtcNow.AddSeconds(1)));
            this.cartsItemsRepositoryMock.Verify(x => x.Update(cartItem), Times.Once);
            this.cartsItemsRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region GetAllCartProductsAsync Tests

        [Test]
        public async Task GetAllCartProductsAsync_UserHasNoCart_ShouldReturnEmptyCollection()
        {
            // Arrange
            string userId = "user-without-cart";

            this.cartRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
                .ReturnsAsync((Cart?)null);

            // Act
            var result = await this.cartService.GetAllCartProductsAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
            this.cartRepositoryMock.Verify(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()), Times.Once);
            this.cartsItemsRepositoryMock.Verify(x => x.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task GetAllCartProductsAsync_UserHasEmptyCart_ShouldReturnEmptyCollection()
        {
            // Arrange
            string userId = "user-with-empty-cart";
            Guid cartId = Guid.NewGuid();
            var userCart = new Cart { Id = cartId, UserId = userId };

            this.cartRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
                .ReturnsAsync(userCart);

            var emptyCartItems = new List<CartItem>().BuildMock();
            this.cartsItemsRepositoryMock
                .Setup(x => x.GetAllAttached())
                .Returns(emptyCartItems);

            // Act
            var result = await this.cartService.GetAllCartProductsAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
            this.cartsItemsRepositoryMock.Verify(x => x.GetAllAttached(), Times.Once);
        }

        [Test]
        public async Task GetAllCartProductsAsync_UserHasCartWithItems_ShouldReturnMappedViewModels()
        {
            // Arrange
            string userId = "user-with-cart-items";
            Guid cartId = Guid.NewGuid();
            Guid productId1 = Guid.NewGuid();
            Guid productId2 = Guid.NewGuid();
            
            var userCart = new Cart { Id = cartId, UserId = userId };
            
            var product1 = new Product 
            { 
                Id = productId1, 
                Name = "Product 1", 
                Price = 10.99m, 
                ImageUrl = "image1.jpg" 
            };
            var product2 = new Product 
            { 
                Id = productId2, 
                Name = "Product 2", 
                Price = 25.50m, 
                ImageUrl = "image2.jpg" 
            };

            var cartItems = new List<CartItem>
            {
                new CartItem 
                { 
                    CartId = cartId, 
                    ProductId = productId1, 
                    Quantity = 2, 
                    IsDeleted = false,
                    Product = product1
                },
                new CartItem 
                { 
                    CartId = cartId, 
                    ProductId = productId2, 
                    Quantity = 1, 
                    IsDeleted = false,
                    Product = product2
                }
            };

            this.cartRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
                .ReturnsAsync(userCart);

            var mockCartItems = cartItems.BuildMock();
            this.cartsItemsRepositoryMock
                .Setup(x => x.GetAllAttached())
                .Returns(mockCartItems);

            // Act
            var result = await this.cartService.GetAllCartProductsAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));

            var resultList = result.ToList();
            var firstItem = resultList.First(x => x.ProductId == productId1);
            var secondItem = resultList.First(x => x.ProductId == productId2);

            Assert.That(firstItem.CartId, Is.EqualTo(cartId));
            Assert.That(firstItem.Quantity, Is.EqualTo(2));
            Assert.That(firstItem.ProductDetails.Name, Is.EqualTo("Product 1"));
            Assert.That(firstItem.ProductDetails.Price, Is.EqualTo(10.99m));

            Assert.That(secondItem.CartId, Is.EqualTo(cartId));
            Assert.That(secondItem.Quantity, Is.EqualTo(1));
            Assert.That(secondItem.ProductDetails.Name, Is.EqualTo("Product 2"));
            Assert.That(secondItem.ProductDetails.Price, Is.EqualTo(25.50m));
        }

        [Test]
        public async Task GetAllCartProductsAsync_UserHasCartWithDeletedItems_ShouldReturnOnlyActiveItems()
        {
            // Arrange
            string userId = "user-with-mixed-cart-items";
            Guid cartId = Guid.NewGuid();
            Guid productId1 = Guid.NewGuid();
            Guid productId2 = Guid.NewGuid();
            
            var userCart = new Cart { Id = cartId, UserId = userId };
            
            var product1 = new Product 
            { 
                Id = productId1, 
                Name = "Active Product", 
                Price = 10.99m, 
                ImageUrl = "image1.jpg" 
            };
            var product2 = new Product 
            { 
                Id = productId2, 
                Name = "Deleted Product", 
                Price = 25.50m, 
                ImageUrl = "image2.jpg" 
            };

            var cartItems = new List<CartItem>
            {
                new CartItem 
                { 
                    CartId = cartId, 
                    ProductId = productId1, 
                    Quantity = 2, 
                    IsDeleted = false,
                    Product = product1
                },
                new CartItem 
                { 
                    CartId = cartId, 
                    ProductId = productId2, 
                    Quantity = 1, 
                    IsDeleted = true,
                    DeletedAt = DateTime.UtcNow,
                    Product = product2
                }
            };

            // Filter to only active items (simulating the EF filtering)
            var activeCartItems = cartItems.Where(ci => !ci.IsDeleted).ToList();

            this.cartRepositoryMock
                .Setup(x => x.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
                .ReturnsAsync(userCart);

            var mockCartItems = activeCartItems.BuildMock();
            this.cartsItemsRepositoryMock
                .Setup(x => x.GetAllAttached())
                .Returns(mockCartItems);

            // Act
            var result = await this.cartService.GetAllCartProductsAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));

            var resultItem = result.First();
            Assert.That(resultItem.ProductId, Is.EqualTo(productId1));
            Assert.That(resultItem.ProductDetails.Name, Is.EqualTo("Active Product"));
        }

        #endregion
    }
}