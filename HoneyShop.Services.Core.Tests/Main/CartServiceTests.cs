namespace HoneyShop.Tests.Services
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.Services.Core.Tests.Main;
    using HoneyShop.ViewModels.Cart;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using System.Linq.Expressions;

    [TestFixture]
    public class CartServiceTests
    {
        private Mock<ICartRepository> mockCartRepository;
        private Mock<ICartsItemsRepository> mockCartsItemsRepository;
        private Mock<IProductRepository> mockProductRepository;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private ICartService cartService;

        private readonly string userId = "test-user-id";
        private readonly Guid cartId = Guid.NewGuid();
        private readonly Guid productId = Guid.NewGuid();
        private ApplicationUser testUser;
        private Product testProduct;
        private Cart testCart;

        [SetUp]
        public void Setup()
        {
            mockCartRepository = new Mock<ICartRepository>();
            mockCartsItemsRepository = new Mock<ICartsItemsRepository>();
            mockProductRepository = new Mock<IProductRepository>();

            Mock<IUserStore<ApplicationUser>> userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            testUser = new ApplicationUser
            {
                Id = userId,
                UserName = "testuser@example.com"
            };

            testProduct = new Product
            {
                Id = productId,
                Name = "Test Honey",
                Price = 10.99m,
                ImageUrl = "test-image-url"
            };

            testCart = new Cart
            {
                Id = cartId,
                UserId = userId,
                IsDeleted = false
            };

            cartService = new CartService(
                mockCartRepository.Object,
                mockCartsItemsRepository.Object,
                mockUserManager.Object,
                mockProductRepository.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        [Test]
        public async Task AddProductToUserCartAsync_UserOrProductNotFound_ReturnsFalse()
        {
            mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            mockProductRepository.Setup(pr => pr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync((Product)null);

            bool result = await cartService.AddProductToUserCartAsync(userId, productId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddProductToUserCartAsync_CreatesNewCartIfUserHasNone()
        {
            mockUserManager.Setup(um => um.FindByIdAsync(userId))
                .ReturnsAsync(testUser);

            mockProductRepository.Setup(pr => pr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(testProduct);

            mockCartRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
                .ReturnsAsync((Cart)null);

            Cart? capturedCart = null;
            mockCartRepository.Setup(cr => cr.AddAsync(It.IsAny<Cart>()))
                .Callback<Cart>(c => capturedCart = c)
                .Returns(Task.CompletedTask);

            mockCartsItemsRepository.Setup(cr => cr.GetAllAttached())
                .Returns(new List<CartItem>().AsQueryable().BuildMock());

            CartItem? capturedItem = null;
            mockCartsItemsRepository.Setup(cr => cr.AddAsync(It.IsAny<CartItem>()))
                .Callback<CartItem>(ci => capturedItem = ci)
                .Returns(Task.CompletedTask);

            bool result = await cartService.AddProductToUserCartAsync(userId, productId);

            Assert.IsTrue(result);

            mockCartRepository.Verify(cr => cr.AddAsync(It.IsAny<Cart>()), Times.Once);
            mockCartRepository.Verify(cr => cr.SaveChangesAsync(), Times.Once);

            Assert.IsNotNull(capturedCart);
            Assert.AreEqual(userId, capturedCart.UserId);
            Assert.IsFalse(capturedCart.IsDeleted);

            mockCartsItemsRepository.Verify(cr => cr.AddAsync(It.IsAny<CartItem>()), Times.Once);
            mockCartsItemsRepository.Verify(cr => cr.SaveChangesAsync(), Times.Once);

            Assert.IsNotNull(capturedItem);
            Assert.AreEqual(capturedCart.Id, capturedItem.CartId);
            Assert.AreEqual(productId, capturedItem.ProductId);
            Assert.AreEqual(1, capturedItem.Quantity);
            Assert.IsFalse(capturedItem.IsDeleted);
        }

        [Test]
        public async Task AddProductToUserCartAsync_AddsNewProductToExistingCart()
        {
            mockUserManager.Setup(um => um.FindByIdAsync(userId))
                .ReturnsAsync(testUser);

            mockProductRepository.Setup(pr => pr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(testProduct);

            mockCartRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
                .ReturnsAsync(testCart);

            mockCartsItemsRepository.Setup(cr => cr.GetAllAttached())
                .Returns(new List<CartItem>().AsQueryable().BuildMock());

            CartItem? capturedItem = null;
            mockCartsItemsRepository.Setup(cr => cr.AddAsync(It.IsAny<CartItem>()))
                .Callback<CartItem>(ci => capturedItem = ci)
                .Returns(Task.CompletedTask);

            bool result = await cartService.AddProductToUserCartAsync(userId, productId);

            Assert.IsTrue(result);

            mockCartRepository.Verify(cr => cr.AddAsync(It.IsAny<Cart>()), Times.Never);

            mockCartsItemsRepository.Verify(cr => cr.AddAsync(It.IsAny<CartItem>()), Times.Once);
            mockCartsItemsRepository.Verify(cr => cr.SaveChangesAsync(), Times.Once);

            Assert.IsNotNull(capturedItem);
            Assert.AreEqual(cartId, capturedItem.CartId);
            Assert.AreEqual(productId, capturedItem.ProductId);
            Assert.AreEqual(1, capturedItem.Quantity);
            Assert.IsFalse(capturedItem.IsDeleted);
        }

        [Test]
        public async Task AddProductToUserCartAsync_IncrementQuantityForExistingProduct()
        {
            mockUserManager.Setup(um => um.FindByIdAsync(userId))
                .ReturnsAsync(testUser);

            mockProductRepository.Setup(pr => pr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(testProduct);

            mockCartRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
                .ReturnsAsync(testCart);

            CartItem? existingCartItem = new CartItem
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = 1,
                IsDeleted = false
            };

            List<CartItem> cartItemsList = new List<CartItem> { existingCartItem };

            mockCartsItemsRepository.Setup(cr => cr.GetAllAttached())
                .Returns(cartItemsList.AsQueryable().BuildMock());

            CartItem? capturedItem = null;
            mockCartsItemsRepository.Setup(cr => cr.Update(It.IsAny<CartItem>()))
                .Callback<CartItem>(ci => capturedItem = ci);

            bool result = await cartService.AddProductToUserCartAsync(userId, productId);

            Assert.IsTrue(result);

            mockCartsItemsRepository.Verify(cr => cr.AddAsync(It.IsAny<CartItem>()), Times.Never);
            mockCartsItemsRepository.Verify(cr => cr.Update(It.IsAny<CartItem>()), Times.Once);
            mockCartsItemsRepository.Verify(cr => cr.SaveChangesAsync(), Times.Once);

            Assert.IsNotNull(capturedItem);
            Assert.AreEqual(2, capturedItem.Quantity);
        }

        [Test]
        public async Task AddProductToUserCartAsync_RestorePreviouslyDeletedProduct()
        {
            mockUserManager.Setup(um => um.FindByIdAsync(userId))
                .ReturnsAsync(testUser);

            mockProductRepository.Setup(pr => pr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(testProduct);

            mockCartRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
                .ReturnsAsync(testCart);

            CartItem? deletedCartItem = new CartItem
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = 2,
                IsDeleted = true,
                DeletedAt = DateTime.UtcNow.AddDays(-1)
            };

            List<CartItem> cartItemsList = new List<CartItem> { deletedCartItem };

            mockCartsItemsRepository.Setup(cr => cr.GetAllAttached())
                .Returns(cartItemsList.AsQueryable().BuildMock());

            CartItem? capturedItem = null;
            mockCartsItemsRepository.Setup(cr => cr.Update(It.IsAny<CartItem>()))
                .Callback<CartItem>(ci => capturedItem = ci);

            bool result = await cartService.AddProductToUserCartAsync(userId, productId);

            Assert.IsTrue(result);

            mockCartsItemsRepository.Verify(cr => cr.Update(It.IsAny<CartItem>()), Times.Once);
            mockCartsItemsRepository.Verify(cr => cr.SaveChangesAsync(), Times.Once);

            Assert.IsNotNull(capturedItem);
            Assert.AreEqual(1, capturedItem.Quantity); // Should reset to 1
            Assert.IsFalse(capturedItem.IsDeleted);
            Assert.IsNull(capturedItem.DeletedAt);
        }

        [Test]
        public async Task DeleteProductFromCartAsync_CartNotFound_ReturnsFalse()
        {
            mockCartRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
                .ReturnsAsync((Cart)null);

            DeleteProductFromCartViewModel? model = new DeleteProductFromCartViewModel
            {
                CartId = cartId,
                ProductId = productId
            };

            bool result = await cartService.DeleteProductFromCartAsync(userId, model);

            Assert.IsFalse(result);
            mockCartsItemsRepository.Verify(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<CartItem, bool>>>()), Times.Never);
        }

        [Test]
        public async Task DeleteProductFromCartAsync_CartIdMismatch_ReturnsFalse()
        {
            mockCartRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
                .ReturnsAsync(testCart);

            DeleteProductFromCartViewModel? model = new DeleteProductFromCartViewModel
            {
                CartId = Guid.NewGuid(), // Different cart ID
                ProductId = productId
            };

            bool result = await cartService.DeleteProductFromCartAsync(userId, model);

            Assert.IsFalse(result);
            mockCartsItemsRepository.Verify(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<CartItem, bool>>>()), Times.Never);
        }

        [Test]
        public async Task DeleteProductFromCartAsync_CartItemNotFound_ReturnsFalse()
        {
            mockCartRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
                .ReturnsAsync(testCart);

            mockCartsItemsRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<CartItem, bool>>>()))
                .ReturnsAsync((CartItem)null);

            DeleteProductFromCartViewModel? model = new DeleteProductFromCartViewModel
            {
                CartId = cartId,
                ProductId = productId
            };

            bool result = await cartService.DeleteProductFromCartAsync(userId, model);

            Assert.IsFalse(result);
            mockCartsItemsRepository.Verify(cr => cr.Update(It.IsAny<CartItem>()), Times.Never);
        }

        [Test]
        public async Task DeleteProductFromCartAsync_Success_MarksItemAsDeleted()
        {
            mockCartRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
                .ReturnsAsync(testCart);

            CartItem? cartItem = new CartItem
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = 1,
                IsDeleted = false
            };

            mockCartsItemsRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<CartItem, bool>>>()))
                .ReturnsAsync(cartItem);

            CartItem? capturedItem = null;
            mockCartsItemsRepository.Setup(cr => cr.Update(It.IsAny<CartItem>()))
                .Callback<CartItem>(ci => capturedItem = ci);

            DeleteProductFromCartViewModel? model = new DeleteProductFromCartViewModel
            {
                CartId = cartId,
                ProductId = productId
            };

            bool result = await cartService.DeleteProductFromCartAsync(userId, model);

            Assert.IsTrue(result);

            mockCartsItemsRepository.Verify(cr => cr.Update(It.IsAny<CartItem>()), Times.Once);
            mockCartsItemsRepository.Verify(cr => cr.SaveChangesAsync(), Times.Once);

            Assert.IsNotNull(capturedItem);
            Assert.IsTrue(capturedItem.IsDeleted);
            Assert.IsNotNull(capturedItem.DeletedAt);
        }

        [Test]
        public async Task GetAllCartProductsAsync_CartNotFound_ReturnsEmptyList()
        {
            mockCartRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
                .ReturnsAsync((Cart)null);

            IEnumerable<GetAllCartItemsViewModel> result = await cartService.GetAllCartProductsAsync(userId);

            Assert.IsEmpty(result);
            mockCartsItemsRepository.Verify(cr => cr.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task GetAllCartProductsAsync_NoCartItems_ReturnsEmptyList()
        {
            mockCartRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
                .ReturnsAsync(testCart);

            mockCartsItemsRepository.Setup(cr => cr.GetAllAttached())
                .Returns(new List<CartItem>().AsQueryable().BuildMock());

            IEnumerable<GetAllCartItemsViewModel> result = await cartService.GetAllCartProductsAsync(userId);

            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllCartProductsAsync_WithItems_ReturnsCorrectViewModels()
        {
            mockCartRepository.Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Cart, bool>>>()))
                .ReturnsAsync(testCart);

            Product product1 = new Product
            {
                Id = productId,
                Name = "Test Honey 1",
                Price = 10.99m,
                ImageUrl = "test-image-url-1"
            };

            Product product2 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Honey 2",
                Price = 15.99m,
                ImageUrl = "test-image-url-2"
            };

            List<CartItem> cartItems = new List<CartItem>
            {
                new CartItem
                {
                    CartId = cartId,
                    ProductId = product1.Id,
                    Product = product1,
                    Quantity = 1,
                    IsDeleted = false
                },
                new CartItem
                {
                    CartId = cartId,
                    ProductId = product2.Id,
                    Product = product2,
                    Quantity = 2,
                    IsDeleted = false
                }
            };

            mockCartsItemsRepository.Setup(cr => cr.GetAllAttached())
                .Returns(cartItems.AsQueryable().BuildMock());

            IEnumerable<GetAllCartItemsViewModel> result = await cartService.GetAllCartProductsAsync(userId);

            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count());

            List<GetAllCartItemsViewModel> resultList = result.ToList();

            Assert.AreEqual(cartId, resultList[0].CartId);
            Assert.AreEqual(product1.Id, resultList[0].ProductId);
            Assert.AreEqual(1, resultList[0].Quantity);
            Assert.AreEqual(product1.Name, resultList[0].ProductDetails.Name);
            Assert.AreEqual(product1.Price, resultList[0].ProductDetails.Price);
            Assert.AreEqual(product1.ImageUrl, resultList[0].ProductDetails.ImageUrl);

            Assert.AreEqual(cartId, resultList[1].CartId);
            Assert.AreEqual(product2.Id, resultList[1].ProductId);
            Assert.AreEqual(2, resultList[1].Quantity);
            Assert.AreEqual(product2.Name, resultList[1].ProductDetails.Name);
            Assert.AreEqual(product2.Price, resultList[1].ProductDetails.Price);
            Assert.AreEqual(product2.ImageUrl, resultList[1].ProductDetails.ImageUrl);
        }
    }
}