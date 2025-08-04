namespace HoneyShop.Tests.Services
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.Services.Core.Tests;
    using HoneyShop.ViewModels.Cart;
    using HoneyShop.ViewModels.Order;
    using MockQueryable.Moq;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<ICartService> mockCartService = null!;
        private Mock<IOrderRepository> mockOrderRepository = null!;
        private Mock<IOrderItemRepository> mockOrderItemRepository = null!;
        private Mock<IOrderStatusRepository> mockOrderStatusRepository = null!;
        private IOrderService orderService = null!;

        private readonly string userId = "test-user-id";
        private readonly Guid orderId = Guid.NewGuid();
        private readonly Guid cartId = Guid.NewGuid();
        private readonly Guid productId1 = Guid.NewGuid();
        private readonly Guid productId2 = Guid.NewGuid();
        private readonly Guid orderStatusId = Guid.NewGuid();
        private List<GetAllCartItemsViewModel> testCartItems = null!;
        private OrderStatus pendingStatus = null!;
        private Order testOrder = null!;
        private CreateOrderViewModel testOrderModel = null!;

        [SetUp]
        public void Setup()
        {
            mockCartService = new Mock<ICartService>();
            mockOrderRepository = new Mock<IOrderRepository>();
            mockOrderItemRepository = new Mock<IOrderItemRepository>();
            mockOrderStatusRepository = new Mock<IOrderStatusRepository>();

            pendingStatus = new OrderStatus
            {
                Id = orderStatusId,
                Name = "Pending",
                IsDeleted = false
            };

            testCartItems = new List<GetAllCartItemsViewModel>
            {
                new GetAllCartItemsViewModel
                {
                    CartId = cartId,
                    ProductId = productId1,
                    Quantity = 2,
                    ProductDetails = new GetProductDetailsForCartViewModel
                    {
                        Id = productId1,
                        Name = "Test Honey 1",
                        Price = 10.99m,
                        ImageUrl = "test-image-1"
                    }
                },
                new GetAllCartItemsViewModel
                {
                    CartId = cartId,
                    ProductId = productId2,
                    Quantity = 1,
                    ProductDetails = new GetProductDetailsForCartViewModel
                    {
                        Id = productId2,
                        Name = "Test Honey 2",
                        Price = 15.99m,
                        ImageUrl = "test-image-2"
                    }
                }
            };

            testOrderModel = new CreateOrderViewModel
            {
                ShippingCity = "Test City",
                ShippingAddress = "123 Test Street"
            };

            testOrder = new Order
            {
                Id = orderId,
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 37.97m,
                OrderStatusId = orderStatusId,
                OrderStatus = pendingStatus,
                ShippingCity = testOrderModel.ShippingCity,
                ShippingAddress = testOrderModel.ShippingAddress,
                IsDeleted = false,
                DeletedAt = null,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = productId1,
                        Product = new Product
                        {
                            Id = productId1,
                            Name = "Test Honey 1",
                            Price = 10.99m
                        },
                        Quantity = 2,
                        UnitPrice = 10.99m,
                        IsDeleted = false
                    },
                    new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = productId2,
                        Product = new Product
                        {
                            Id = productId2,
                            Name = "Test Honey 2",
                            Price = 15.99m
                        },
                        Quantity = 1,
                        UnitPrice = 15.99m,
                        IsDeleted = false
                    }
                }
            };

            orderService = new OrderService(
                mockCartService.Object,
                mockOrderRepository.Object,
                mockOrderItemRepository.Object,
                mockOrderStatusRepository.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        [Test]
        public async Task CreateOrderAsync_EmptyCart_ThrowsInvalidOperationException()
        {
            mockCartService.Setup(cs => cs.GetAllCartProductsAsync(userId))
                .ReturnsAsync(new List<GetAllCartItemsViewModel>());

            var ex = Assert.ThrowsAsync<InvalidOperationException>(
                async () => await orderService.CreateOrderAsync(userId, testOrderModel));

            Assert.That(ex, Is.Not.Null);
            Assert.That(ex!.Message, Is.EqualTo("Cannot create an order with an empty cart."));
        }

        [Test]
        public async Task CreateOrderAsync_PendingStatusNotFound_ThrowsInvalidOperationException()
        {
            mockCartService.Setup(cs => cs.GetAllCartProductsAsync(userId))
                .ReturnsAsync(testCartItems);

            mockOrderStatusRepository.Setup(osr => osr.GetAllAttached())
                .Returns(new List<OrderStatus>().AsQueryable().BuildMock());

            var ex = Assert.ThrowsAsync<InvalidOperationException>(
                async () => await orderService.CreateOrderAsync(userId, testOrderModel));

            Assert.That(ex, Is.Not.Null);
            Assert.That(ex!.Message, Is.EqualTo("Required order status 'Pending' not found."));
        }

        [Test]
        public async Task CreateOrderAsync_ValidOrder_CreatesOrderAndItems()
        {
            mockCartService.Setup(cs => cs.GetAllCartProductsAsync(userId))
                .ReturnsAsync(testCartItems);

            List<OrderStatus> orderStatuses = new List<OrderStatus> { pendingStatus };
            mockOrderStatusRepository.Setup(osr => osr.GetAllAttached())
                .Returns(orderStatuses.AsQueryable().BuildMock());

            Order? capturedOrder = null;
            mockOrderRepository.Setup(or => or.AddAsync(It.IsAny<Order>()))
                .Callback<Order>(o => capturedOrder = o)
                .Returns(Task.CompletedTask);

            List<OrderItem> capturedOrderItems = new List<OrderItem>();
            mockOrderItemRepository.Setup(oir => oir.AddAsync(It.IsAny<OrderItem>()))
                .Callback<OrderItem>(oi => capturedOrderItems.Add(oi))
                .Returns(Task.CompletedTask);

            mockCartService.Setup(cs => cs.DeleteProductFromCartAsync(
                    It.IsAny<string>(), It.IsAny<DeleteProductFromCartViewModel>()))
                .ReturnsAsync(true);

            Guid result = await orderService.CreateOrderAsync(userId, testOrderModel);

            Assert.That(capturedOrder, Is.Not.Null);
            Assert.That(capturedOrder!.UserId, Is.EqualTo(userId));
            Assert.That(capturedOrder.ShippingCity, Is.EqualTo(testOrderModel.ShippingCity));
            Assert.That(capturedOrder.ShippingAddress, Is.EqualTo(testOrderModel.ShippingAddress));
            Assert.That(capturedOrder.OrderStatusId, Is.EqualTo(pendingStatus.Id));
            Assert.That(capturedOrder.TotalAmount, Is.EqualTo(37.97m));
            Assert.That(capturedOrder.IsDeleted, Is.False);

            mockOrderRepository.Verify(or => or.AddAsync(It.IsAny<Order>()), Times.Once);
            mockOrderRepository.Verify(or => or.SaveChangesAsync(), Times.Once);

            Assert.That(capturedOrderItems.Count, Is.EqualTo(2));

            Assert.That(capturedOrderItems[0].OrderId, Is.EqualTo(capturedOrder.Id));
            Assert.That(capturedOrderItems[0].ProductId, Is.EqualTo(productId1));
            Assert.That(capturedOrderItems[0].Quantity, Is.EqualTo(2));
            Assert.That(capturedOrderItems[0].UnitPrice, Is.EqualTo(10.99m));
            Assert.That(capturedOrderItems[0].IsDeleted, Is.False);

            Assert.That(capturedOrderItems[1].OrderId, Is.EqualTo(capturedOrder.Id));
            Assert.That(capturedOrderItems[1].ProductId, Is.EqualTo(productId2));
            Assert.That(capturedOrderItems[1].Quantity, Is.EqualTo(1));
            Assert.That(capturedOrderItems[1].UnitPrice, Is.EqualTo(15.99m));
            Assert.That(capturedOrderItems[1].IsDeleted, Is.False);

            mockOrderItemRepository.Verify(oir => oir.AddAsync(It.IsAny<OrderItem>()), Times.Exactly(2));
            mockOrderItemRepository.Verify(oir => oir.SaveChangesAsync(), Times.Once);

            mockCartService.Verify(cs => cs.DeleteProductFromCartAsync(
                It.IsAny<string>(), It.IsAny<DeleteProductFromCartViewModel>()), Times.Exactly(2));

            Assert.That(result, Is.EqualTo(capturedOrder.Id));
        }

        [Test]
        public async Task GetOrderConfirmationAsync_OrderNotFound_ReturnsNull()
        {
            mockOrderRepository.Setup(or => or.GetAllAttached())
                .Returns(new List<Order>().AsQueryable().BuildMock());

            OrderConfirmationViewModel? result = await orderService.GetOrderConfirmationAsync(orderId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetOrderConfirmationAsync_OrderFound_ReturnsViewModel()
        {
            List<Order> orders = new List<Order> { testOrder };
            mockOrderRepository.Setup(or => or.GetAllAttached())
                .Returns(orders.AsQueryable().BuildMock());

            OrderConfirmationViewModel? result = await orderService.GetOrderConfirmationAsync(orderId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.OrderId, Is.EqualTo(orderId));
            Assert.That(result.ShippingCity, Is.EqualTo(testOrder.ShippingCity));
            Assert.That(result.ShippingAddress, Is.EqualTo(testOrder.ShippingAddress));
            Assert.That(result.TotalAmount, Is.EqualTo(testOrder.TotalAmount));
            Assert.That(result.OrderDate, Is.EqualTo(testOrder.OrderDate));
            Assert.That(result.OrderStatus, Is.EqualTo(testOrder.OrderStatus.Name));
        }

        [Test]
        public async Task GetUserOrdersAsync_NoOrders_ReturnsEmptyList()
        {
            mockOrderRepository.Setup(or => or.GetAllAttached())
                .Returns(new List<Order>().AsQueryable().BuildMock());

            IEnumerable<GetAllOrdersForUserViewModel> result = await orderService.GetUserOrdersAsync(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetUserOrdersAsync_WithOrders_ReturnsOrderViewModels()
        {
            List<Order> orders = new List<Order> { testOrder };
            mockOrderRepository.Setup(or => or.GetAllAttached())
                .Returns(orders.AsQueryable().BuildMock());

            IEnumerable<GetAllOrdersForUserViewModel> result = await orderService.GetUserOrdersAsync(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));

            GetAllOrdersForUserViewModel orderViewModel = result.First();
            Assert.That(orderViewModel.Id, Is.EqualTo(orderId));
            Assert.That(orderViewModel.OrderDate, Is.EqualTo(testOrder.OrderDate));
            Assert.That(orderViewModel.TotalAmount, Is.EqualTo(testOrder.TotalAmount));
            Assert.That(orderViewModel.OrderStatus, Is.EqualTo(testOrder.OrderStatus.Name));
            Assert.That(orderViewModel.ShippingCity, Is.EqualTo(testOrder.ShippingCity));
            Assert.That(orderViewModel.ShippingAddress, Is.EqualTo(testOrder.ShippingAddress));

            Assert.That(orderViewModel.OrderItems.Count(), Is.EqualTo(2));

            OrderItemViewModel firstItem = orderViewModel.OrderItems.First();
            Assert.That(firstItem.ProductId, Is.EqualTo(productId1));
            Assert.That(firstItem.ProductName, Is.EqualTo("Test Honey 1"));
            Assert.That(firstItem.ProductPrice, Is.EqualTo(10.99m));
            Assert.That(firstItem.Quantity, Is.EqualTo(2));

            OrderItemViewModel secondItem = orderViewModel.OrderItems.Skip(1).First();
            Assert.That(secondItem.ProductId, Is.EqualTo(productId2));
            Assert.That(secondItem.ProductName, Is.EqualTo("Test Honey 2"));
            Assert.That(secondItem.ProductPrice, Is.EqualTo(15.99m));
            Assert.That(secondItem.Quantity, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUserOrdersAsync_FiltersDeletedOrders()
        {
            Order deletedOrder = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OrderStatus = pendingStatus,
                IsDeleted = true,
                OrderItems = new List<OrderItem>()
            };

            List<Order> orders = new List<Order> { testOrder, deletedOrder };
            mockOrderRepository.Setup(or => or.GetAllAttached())
                .Returns(orders.AsQueryable().BuildMock());

            IEnumerable<GetAllOrdersForUserViewModel> result = await orderService.GetUserOrdersAsync(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(orderId));
        }

        [Test]
        public async Task GetUserOrdersAsync_FiltersDeletedOrderItems()
        {
            Order orderWithDeletedItem = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 10.99m,
                OrderStatusId = orderStatusId,
                OrderStatus = pendingStatus,
                ShippingCity = "Test City",
                ShippingAddress = "Test Address",
                IsDeleted = false,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = productId1,
                        Product = new Product
                        {
                            Id = productId1,
                            Name = "Test Honey 1",
                            Price = 10.99m
                        },
                        Quantity = 1,
                        UnitPrice = 10.99m,
                        IsDeleted = false
                    },
                    new OrderItem
                    {
                        ProductId = productId2,
                        Product = new Product
                        {
                            Id = productId2,
                            Name = "Test Honey 2",
                            Price = 15.99m
                        },
                        Quantity = 1,
                        UnitPrice = 15.99m,
                        IsDeleted = true 
                    }
                }
            };

            List<Order> orders = new List<Order> { orderWithDeletedItem };
            mockOrderRepository.Setup(or => or.GetAllAttached())
                .Returns(orders.AsQueryable().BuildMock());

            IEnumerable<GetAllOrdersForUserViewModel> result = await orderService.GetUserOrdersAsync(userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().OrderItems.Count(), Is.EqualTo(1));
            Assert.That(result.First().OrderItems.First().ProductId, Is.EqualTo(productId1));
        }
    }
}