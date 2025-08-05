namespace HoneyShop.Services.Core.Tests.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin;
    using HoneyShop.ViewModels.Admin.Home;
    using HoneyShop.ViewModels.Admin.OrderManagment;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class AdminOrderServiceTests
    {
        private Mock<IOrderRepository> mockOrderRepository;
        private Mock<IOrderStatusRepository> mockOrderStatusRepository;
        private Core.Admin.Contracts.IOrderService orderService;

        private readonly Guid orderId1 = Guid.NewGuid();
        private readonly Guid orderId2 = Guid.NewGuid();
        private readonly Guid userId1 = Guid.NewGuid();
        private readonly Guid userId2 = Guid.NewGuid();
        private readonly Guid statusId1 = Guid.NewGuid();
        private readonly Guid statusId2 = Guid.NewGuid();
        private readonly Guid productId1 = Guid.NewGuid();
        private readonly Guid productId2 = Guid.NewGuid();

        private List<Order> testOrders;
        private List<OrderStatus> testOrderStatuses;
        private ApplicationUser testUser1;
        private ApplicationUser testUser2;
        private DateTime currentDate = DateTime.Parse("2025-08-04 19:17:19");

        [SetUp]
        public void Setup()
        {
            mockOrderRepository = new Mock<IOrderRepository>();
            mockOrderStatusRepository = new Mock<IOrderStatusRepository>();

            testOrderStatuses = new List<OrderStatus>
            {
                new OrderStatus
                {
                    Id = statusId1,
                    Name = "Pending",
                    Description = "Order is pending",
                    IsDeleted = false
                },
                new OrderStatus
                {
                    Id = statusId2,
                    Name = "Confirmed",
                    Description = "Order is confirmed",
                    IsDeleted = false
                },
                new OrderStatus
                {
                    Id = Guid.NewGuid(),
                    Name = "Sent",
                    Description = "Order has been sent",
                    IsDeleted = false
                },
                new OrderStatus
                {
                    Id = Guid.NewGuid(),
                    Name = "Finished",
                    Description = "Order is finished",
                    IsDeleted = false
                }
            };

            testUser1 = new ApplicationUser
            {
                Id = userId1.ToString(),
                UserName = "testuser1",
                Email = "testuser1@example.com"
            };

            testUser2 = new ApplicationUser
            {
                Id = userId2.ToString(),
                UserName = "testuser2",
                Email = "testuser2@example.com"
            };

            testOrders = new List<Order>
            {
                new Order
                {
                    Id = orderId1,
                    UserId = userId1.ToString(),
                    User = testUser1,
                    OrderDate = currentDate.AddDays(-1),
                    TotalAmount = 50.99m,
                    OrderStatusId = statusId1,
                    OrderStatus = testOrderStatuses[0],
                    ShippingCity = "Sofia",
                    ShippingAddress = "Test Address 1",
                    IsDeleted = false,
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            OrderId = orderId1,
                            ProductId = productId1,
                            Product = new Product
                            {
                                Id = productId1,
                                Name = "Honey Jar",
                                Price = 10.99m
                            },
                            Quantity = 3,
                            UnitPrice = 10.99m,
                            IsDeleted = false
                        },
                        new OrderItem
                        {
                            OrderId = orderId1,
                            ProductId = productId2,
                            Product = new Product
                            {
                                Id = productId2,
                                Name = "Beeswax Candle",
                                Price = 18.02m
                            },
                            Quantity = 1,
                            UnitPrice = 18.02m,
                            IsDeleted = false
                        }
                    }
                },
                new Order
                {
                    Id = orderId2,
                    UserId = userId2.ToString(),
                    User = testUser2,
                    OrderDate = currentDate.AddDays(-5),
                    TotalAmount = 25.99m,
                    OrderStatusId = statusId2,
                    OrderStatus = testOrderStatuses[1],
                    ShippingCity = "Plovdiv",
                    ShippingAddress = "Test Address 2",
                    IsDeleted = false,
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            OrderId = orderId2,
                            ProductId = productId2,
                            Product = new Product
                            {
                                Id = productId2,
                                Name = "Beeswax Candle",
                                Price = 25.99m
                            },
                            Quantity = 1,
                            UnitPrice = 25.99m,
                            IsDeleted = false
                        }
                    }
                }
            };

            orderService = new OrderService(
                mockOrderRepository.Object,
                mockOrderStatusRepository.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        [Test]
        public async Task GetAllOrdersAsync_ReturnsAllOrders()
        {
            IQueryable<Order> mockQueryable = testOrders.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<OrderViewModel> result = await orderService.GetAllOrdersAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            List<OrderViewModel> orders = result.ToList();

            Assert.AreEqual(orderId1, orders[0].Id);
            Assert.AreEqual(currentDate.AddDays(-1), orders[0].OrderDate);
            Assert.AreEqual(50.99m, orders[0].TotalAmount);
            Assert.AreEqual("Pending", orders[0].OrderStatus);
            Assert.AreEqual(statusId1, orders[0].OrderStatusId);
            Assert.AreEqual("testuser1@example.com", orders[0].UserEmail);
            Assert.AreEqual("testuser1", orders[0].UserName);
            Assert.AreEqual("Sofia", orders[0].ShippingCity);
            Assert.AreEqual("Test Address 1", orders[0].ShippingAddress);

            Assert.AreEqual(orderId2, orders[1].Id);
            Assert.AreEqual(currentDate.AddDays(-5), orders[1].OrderDate);
            Assert.AreEqual(25.99m, orders[1].TotalAmount);
            Assert.AreEqual("Confirmed", orders[1].OrderStatus);
            Assert.AreEqual(statusId2, orders[1].OrderStatusId);
            Assert.AreEqual("testuser2@example.com", orders[1].UserEmail);
            Assert.AreEqual("testuser2", orders[1].UserName);
            Assert.AreEqual("Plovdiv", orders[1].ShippingCity);
            Assert.AreEqual("Test Address 2", orders[1].ShippingAddress);
        }

        [Test]
        public async Task GetAllOrdersAsync_ExcludesDeletedOrders()
        {
            Order deletedOrder = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId1.ToString(),
                User = testUser1,
                OrderDate = currentDate.AddDays(-3),
                TotalAmount = 30.50m,
                OrderStatusId = statusId1,
                OrderStatus = testOrderStatuses[0],
                ShippingCity = "Sofia",
                ShippingAddress = "Test Address 3",
                IsDeleted = true
            };

            List<Order> allOrders = testOrders.Concat(new[] { deletedOrder }).ToList();
            IQueryable<Order> mockQueryable = allOrders.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<OrderViewModel> result = await orderService.GetAllOrdersAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count()); 

            Assert.IsFalse(result.Any(o => o.Id == deletedOrder.Id));
        }

        [Test]
        public async Task GetOrderDetailsAsync_OrderFound_ReturnsOrderDetails()
        {
            IQueryable<Order> mockQueryable = testOrders.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            OrderDetailsViewModel? result = await orderService.GetOrderDetailsAsync(orderId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(orderId1, result.Id);
            Assert.AreEqual(currentDate.AddDays(-1), result.OrderDate);
            Assert.AreEqual(50.99m, result.TotalAmount);
            Assert.AreEqual("Pending", result.OrderStatus);
            Assert.AreEqual(statusId1, result.OrderStatusId);
            Assert.AreEqual("testuser1@example.com", result.UserEmail);
            Assert.AreEqual("testuser1", result.UserName);
            Assert.AreEqual(userId1.ToString(), result.UserId);
            Assert.AreEqual("Sofia", result.ShippingCity);
            Assert.AreEqual("Test Address 1", result.ShippingAddress);

            Assert.AreEqual(2, result.OrderItems.Count());

            List<OrderItemViewModel> orderItems = result.OrderItems.ToList();
            Assert.AreEqual(productId1, orderItems[0].ProductId);
            Assert.AreEqual("Honey Jar", orderItems[0].ProductName);
            Assert.AreEqual(10.99m, orderItems[0].UnitPrice);
            Assert.AreEqual(3, orderItems[0].Quantity);
            Assert.AreEqual(32.97m, orderItems[0].TotalPrice);

            Assert.AreEqual(productId2, orderItems[1].ProductId);
            Assert.AreEqual("Beeswax Candle", orderItems[1].ProductName);
            Assert.AreEqual(18.02m, orderItems[1].UnitPrice);
            Assert.AreEqual(1, orderItems[1].Quantity);
            Assert.AreEqual(18.02m, orderItems[1].TotalPrice);
        }

        [Test]
        public async Task GetOrderDetailsAsync_OrderNotFound_ReturnsNull()
        {
            IQueryable<Order> mockQueryable = testOrders.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            OrderDetailsViewModel? result = await orderService.GetOrderDetailsAsync(Guid.NewGuid());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetOrderDetailsAsync_ExcludesDeletedItems()
        {
            Order orderWithDeletedItem = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId1.ToString(),
                User = testUser1,
                OrderDate = currentDate.AddDays(-2),
                TotalAmount = 20.99m,
                OrderStatusId = statusId1,
                OrderStatus = testOrderStatuses[0],
                ShippingCity = "Sofia",
                ShippingAddress = "Test Address 4",
                IsDeleted = false,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = productId1,
                        Product = new Product
                        {
                            Id = productId1,
                            Name = "Honey Jar"
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
                            Name = "Beeswax Candle"
                        },
                        Quantity = 1,
                        UnitPrice = 18.02m,
                        IsDeleted = true
                    }
                }
            };

            List<Order> allOrders = testOrders.Concat(new[] { orderWithDeletedItem }).ToList();
            IQueryable<Order> mockQueryable = allOrders.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            OrderDetailsViewModel? result = await orderService.GetOrderDetailsAsync(orderWithDeletedItem.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.OrderItems.Count());
            Assert.AreEqual(productId1, result.OrderItems.First().ProductId);
        }

        [Test]
        public async Task GetAllOrderStatusesAsync_ReturnsAllStatuses()
        {
            IQueryable<OrderStatus> mockQueryable = testOrderStatuses.AsQueryable().BuildMock();

            mockOrderStatusRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<OrderStatusViewModel> result = await orderService.GetAllOrderStatusesAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());

            List<OrderStatusViewModel> statuses = result.ToList();
            Assert.AreEqual(statusId1, statuses[0].Id);
            Assert.AreEqual("Pending", statuses[0].Name);
            Assert.AreEqual("Order is pending", statuses[0].Description);

            Assert.AreEqual(statusId2, statuses[1].Id);
            Assert.AreEqual("Confirmed", statuses[1].Name);
            Assert.AreEqual("Order is confirmed", statuses[1].Description);

            Assert.AreEqual("Sent", statuses[2].Name);
            Assert.AreEqual("Order has been sent", statuses[2].Description);

            Assert.AreEqual("Finished", statuses[3].Name);
            Assert.AreEqual("Order is finished", statuses[3].Description);
        }

        [Test]
        public async Task GetAllOrderStatusesAsync_ExcludesDeletedStatuses()
        {
            OrderStatus deletedStatus = new OrderStatus
            {
                Id = Guid.NewGuid(),
                Name = "Canceled",
                Description = "Order is canceled",
                IsDeleted = true
            };

            List<OrderStatus> allStatuses = testOrderStatuses.Concat(new[] { deletedStatus }).ToList();
            IQueryable<OrderStatus> mockQueryable = allStatuses.AsQueryable().BuildMock();

            mockOrderStatusRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<OrderStatusViewModel> result = await orderService.GetAllOrderStatusesAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());
            Assert.IsFalse(result.Any(s => s.Name == "Canceled"));
        }

        [Test]
        public async Task UpdateOrderStatusAsync_OrderAndStatusFound_UpdatesStatus()
        {
            IQueryable<Order> mockOrderQueryable = testOrders.AsQueryable().BuildMock();
            IQueryable<OrderStatus> mockStatusQueryable = testOrderStatuses.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockOrderQueryable);

            mockOrderStatusRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockStatusQueryable);

            Order orderToUpdate = testOrders.First(o => o.Id == orderId1);

            bool result = await orderService.UpdateOrderStatusAsync(orderId1, statusId2);

            Assert.IsTrue(result);
            mockOrderRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
            Assert.AreEqual(statusId2, orderToUpdate.OrderStatusId);
        }

        [Test]
        public async Task UpdateOrderStatusAsync_OrderNotFound_ReturnsFalse()
        {
            IQueryable<Order> mockOrderQueryable = testOrders.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockOrderQueryable);

            bool result = await orderService.UpdateOrderStatusAsync(Guid.NewGuid(), statusId2);

            Assert.IsFalse(result);
            mockOrderRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task UpdateOrderStatusAsync_StatusNotFound_ReturnsFalse()
        {
            IQueryable<Order> mockOrderQueryable = testOrders.AsQueryable().BuildMock();
            IQueryable<OrderStatus> mockStatusQueryable = testOrderStatuses.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockOrderQueryable);

            mockOrderStatusRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockStatusQueryable);

            bool result = await orderService.UpdateOrderStatusAsync(orderId1, Guid.NewGuid());

            Assert.IsFalse(result);
            mockOrderRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task GetOrderStatisticsAsync_CalculatesCorrectStatistics()
        {
            DateTime currentDate = DateTime.Parse("2025-08-05 18:06:02");
            DateTime today = currentDate.Date;
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);

            Order todayOrder1 = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = currentDate,
                TotalAmount = 40.00m,
                OrderStatusId = statusId1,
                OrderStatus = testOrderStatuses[0],
                IsDeleted = false
            };

            Order todayOrder2 = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = currentDate,
                TotalAmount = 100.00m,
                OrderStatusId = statusId1,
                OrderStatus = testOrderStatuses[0],
                IsDeleted = false
            };

            Order thisWeekOrder = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = today.AddDays(-1),
                TotalAmount = 60.00m,
                OrderStatusId = statusId2,
                OrderStatus = testOrderStatuses[1],
                IsDeleted = false
            };

            Order thisMonthOrder = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = startOfMonth,
                TotalAmount = 80.00m,
                OrderStatusId = testOrderStatuses[2].Id,
                OrderStatus = testOrderStatuses[2],
                IsDeleted = false
            };

            Order oldOrder = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = startOfMonth.AddDays(-5),
                TotalAmount = 200.00m,
                OrderStatusId = testOrderStatuses[3].Id,
                OrderStatus = testOrderStatuses[3],
                IsDeleted = false
            };

            List<Order> allOrders = new List<Order> 
            {
                todayOrder1,
                todayOrder2,
                thisWeekOrder,
                thisMonthOrder,
                oldOrder
            };

            IQueryable<Order> mockQueryable = allOrders.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            DashboardOrderStats stats = await orderService.GetOrderStatisticsAsync();

            Assert.IsNotNull(stats);

            Assert.AreEqual(5, stats.TotalOrders);

            Assert.AreEqual(2, stats.PendingOrders);
            Assert.AreEqual(1, stats.ConfirmedOrders);
            Assert.AreEqual(1, stats.SentOrders);
            Assert.AreEqual(1, stats.FinishedOrders);

            decimal expectedTotalSales = todayOrder1.TotalAmount + todayOrder2.TotalAmount +
                                       thisWeekOrder.TotalAmount + thisMonthOrder.TotalAmount +
                                       oldOrder.TotalAmount;
            Assert.AreEqual(expectedTotalSales, stats.TotalSales);

            decimal expectedDailySales = todayOrder1.TotalAmount + todayOrder2.TotalAmount;
            Assert.AreEqual(expectedDailySales, stats.DailySales);

            decimal expectedWeeklySales = todayOrder1.TotalAmount + todayOrder2.TotalAmount + thisWeekOrder.TotalAmount;
            Assert.AreEqual(expectedWeeklySales, stats.WeeklySales);

            decimal expectedMonthlySales = todayOrder1.TotalAmount + todayOrder2.TotalAmount +
                                         thisWeekOrder.TotalAmount + thisMonthOrder.TotalAmount;
            Assert.AreEqual(expectedMonthlySales, stats.MonthlySales);
        }

        [Test]
        public async Task GetRecentOrdersAsync_ReturnsSpecifiedNumberOfOrders()
        {
            List<Order> additionalOrders = new List<Order>();
            for (int i = 0; i < 10; i++)
            {
                additionalOrders.Add(new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId1.ToString(),
                    User = testUser1,
                    OrderDate = currentDate.AddHours(-i),
                    TotalAmount = 10.00m * i,
                    OrderStatusId = statusId1,
                    OrderStatus = testOrderStatuses[0],
                    IsDeleted = false
                });
            }

            List<Order> allOrders = additionalOrders.Concat(testOrders).ToList();
            IQueryable<Order> mockQueryable = allOrders.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

           IEnumerable<RecentOrderViewModel> result = await orderService.GetRecentOrdersAsync(5);

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count());

            List<RecentOrderViewModel> orders = result.ToList();
            for (int i = 0; i < orders.Count - 1; i++)
            {
                Assert.IsTrue(orders[i].OrderDate >= orders[i + 1].OrderDate);
            }
        }

        [Test]
        public async Task GetRecentOrdersAsync_WithDefaultCount_ReturnsFiveOrders()
        {
            List<Order> additionalOrders = new List<Order>();
            for (int i = 0; i < 10; i++)
            {
                additionalOrders.Add(new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId1.ToString(),
                    User = testUser1,
                    OrderDate = currentDate.AddHours(-i),
                    TotalAmount = 10.00m * i,
                    OrderStatusId = statusId1,
                    OrderStatus = testOrderStatuses[0],
                    IsDeleted = false
                });
            }

            List<Order> allOrders = additionalOrders.Concat(testOrders).ToList();
            IQueryable<Order> mockQueryable = allOrders.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<RecentOrderViewModel> result = await orderService.GetRecentOrdersAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count());
        }

        [Test]
        public async Task GetRecentOrdersAsync_ReturnsCorrectViewModel()
        {
            IQueryable<Order> mockQueryable = testOrders.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<RecentOrderViewModel> result = await orderService.GetRecentOrdersAsync(2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            List<RecentOrderViewModel> orders = result.ToList();
            Assert.AreEqual(orderId1, orders[0].Id);
            Assert.AreEqual("testuser1", orders[0].CustomerName);
            Assert.AreEqual(currentDate.AddDays(-1), orders[0].OrderDate);
            Assert.AreEqual(50.99m, orders[0].TotalAmount);
            Assert.AreEqual("Pending", orders[0].OrderStatus);

            Assert.AreEqual(orderId2, orders[1].Id);
            Assert.AreEqual("testuser2", orders[1].CustomerName);
            Assert.AreEqual(currentDate.AddDays(-5), orders[1].OrderDate);
            Assert.AreEqual(25.99m, orders[1].TotalAmount);
            Assert.AreEqual("Confirmed", orders[1].OrderStatus);
        }

        [Test]
        public async Task GetRecentOrdersAsync_WithNoUsername_UsesEmail()
        {
            ApplicationUser userWithoutUsername = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = null,
                Email = "noname@example.com"
            };

            Order orderWithoutUsername = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userWithoutUsername.Id,
                User = userWithoutUsername,
                OrderDate = currentDate,
                TotalAmount = 30.00m,
                OrderStatusId = statusId1,
                OrderStatus = testOrderStatuses[0],
                IsDeleted = false
            };

            List<Order> allOrders = testOrders.Concat(new[] { orderWithoutUsername }).ToList();
            IQueryable<Order> mockQueryable = allOrders.AsQueryable().BuildMock();

            mockOrderRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<RecentOrderViewModel> result = await orderService.GetRecentOrdersAsync(3);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());

            RecentOrderViewModel? orderWithEmailAsName = result.FirstOrDefault(o => o.Id == orderWithoutUsername.Id);
            Assert.IsNotNull(orderWithEmailAsName);
            Assert.AreEqual("noname@example.com", orderWithEmailAsName.CustomerName);
        }
    }
}