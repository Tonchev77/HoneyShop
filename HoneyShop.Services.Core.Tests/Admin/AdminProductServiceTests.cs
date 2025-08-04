namespace HoneyShop.Services.Core.Tests.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin;
    using HoneyShop.ViewModels.Admin.Home;
    using HoneyShop.ViewModels.Admin.ProductManagment;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using IProductService = Core.Admin.Contracts.IProductService;

    [TestFixture]
    public class AdminProductServiceTests
    {
        private Mock<IProductRepository> mockProductRepository;
        private Mock<ICategoryRepository> mockCategoryRepository;
        private Mock<IOrderItemRepository> mockOrderItemRepository;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private IProductService productService;

        private readonly Guid productId1 = Guid.NewGuid();
        private readonly Guid productId2 = Guid.NewGuid();
        private readonly Guid categoryId1 = Guid.NewGuid();
        private readonly Guid categoryId2 = Guid.NewGuid();
        private readonly string userId1 = Guid.NewGuid().ToString();
        private readonly string userId2 = Guid.NewGuid().ToString();
        private readonly DateTime currentDate = DateTime.Parse("2025-08-04 19:52:33");

        private List<Product> testProducts;
        private List<Category> testCategories;
        private List<OrderItem> testOrderItems;
        private List<ApplicationUser> testUsers;

        [SetUp]
        public void Setup()
        {
            mockProductRepository = new Mock<IProductRepository>();
            mockCategoryRepository = new Mock<ICategoryRepository>();
            mockOrderItemRepository = new Mock<IOrderItemRepository>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            testCategories = new List<Category>
            {
                new Category
                {
                    Id = categoryId1,
                    Name = "Honey Products",
                    Description = "All types of honey products",
                    IsDeleted = false
                },
                new Category
                {
                    Id = categoryId2,
                    Name = "Beeswax Products",
                    Description = "Products made from beeswax",
                    IsDeleted = false
                }
            };

            testUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = userId1,
                    UserName = "Tonchev77",
                    Email = "admin@honeyshop.com"
                },
                new ApplicationUser
                {
                    Id = userId2,
                    UserName = "user2",
                    Email = "user2@example.com"
                }
            };

            testProducts = new List<Product>
            {
                new Product
                {
                    Id = productId1,
                    Name = "Organic Honey",
                    Description = "Pure organic honey from mountain flowers",
                    Price = 10.99m,
                    ImageUrl = "honey1.jpg",
                    IsActive = true,
                    CategoryId = categoryId1,
                    Category = testCategories[0],
                    CreatedAt = currentDate.AddDays(-30),
                    CreatorId = userId1,
                    IsDeleted = false,
                    ProductStocks = new List<ProductStock>
                    {
                        new ProductStock
                        {
                            ProductId = productId1,
                            Quantity = 15,
                            IsDeleted = false
                        }
                    }
                },
                new Product
                {
                    Id = productId2,
                    Name = "Beeswax Candle",
                    Description = "Handmade beeswax candle",
                    Price = 15.99m,
                    ImageUrl = "candle1.jpg",
                    IsActive = true,
                    CategoryId = categoryId2,
                    Category = testCategories[1],
                    CreatedAt = currentDate.AddDays(-15),
                    CreatorId = userId1,
                    IsDeleted = false,
                    ProductStocks = new List<ProductStock>
                    {
                        new ProductStock
                        {
                            ProductId = productId2,
                            Quantity = 5,
                            IsDeleted = false
                        }
                    }
                }
            };

            Order order1 = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId2,
                OrderDate = currentDate.AddDays(-10),
                IsDeleted = false
            };

            Order order2 = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId2,
                OrderDate = currentDate.AddDays(-5),
                IsDeleted = false
            };

            testOrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    OrderId = order1.Id,
                    Order = order1,
                    ProductId = productId1,
                    Product = testProducts[0],
                    Quantity = 3,
                    UnitPrice = 10.99m,
                    IsDeleted = false
                },
                new OrderItem
                {
                    OrderId = order1.Id,
                    Order = order1,
                    ProductId = productId2,
                    Product = testProducts[1],
                    Quantity = 1,
                    UnitPrice = 15.99m,
                    IsDeleted = false
                },
                new OrderItem
                {
                    OrderId = order2.Id,
                    Order = order2,
                    ProductId = productId1,
                    Product = testProducts[0],
                    Quantity = 2,
                    UnitPrice = 10.99m,
                    IsDeleted = false
                }
            };

            productService = new ProductService(
                mockProductRepository.Object,
                mockCategoryRepository.Object,
                mockUserManager.Object,
                mockOrderItemRepository.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        [Test]
        public async Task AddProductAsync_UserNotFound_ReturnsFalse()
        {
            mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            AddProductViewModel addModel = new AddProductViewModel
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                ImageUrl = "test.jpg",
                IsActive = true,
                CategoryId = categoryId1
            };

            bool result = await productService.AddProductAsync(userId1, addModel);

            Assert.IsFalse(result);
            mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Never);
            mockProductRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task AddProductAsync_CategoryNotFound_ReturnsFalse()
        {
            mockUserManager.Setup(um => um.FindByIdAsync(userId1))
                .ReturnsAsync(testUsers[0]);

            mockCategoryRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Category)null);

            AddProductViewModel addModel = new AddProductViewModel
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                ImageUrl = "test.jpg",
                IsActive = true,
                CategoryId = categoryId1
            };

            bool result = await productService.AddProductAsync(userId1, addModel);

            Assert.IsFalse(result);
            mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Never);
            mockProductRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task AddProductAsync_ValidData_AddsProduct()
        {
            mockUserManager.Setup(um => um.FindByIdAsync(userId1))
                .ReturnsAsync(testUsers[0]);

            mockCategoryRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(testCategories[0]);

            AddProductViewModel addModel = new AddProductViewModel
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                ImageUrl = "test.jpg",
                IsActive = true,
                CategoryId = categoryId1
            };

            Product? capturedProduct = null;
            mockProductRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .Callback<Product>(p => capturedProduct = p)
                .Returns(Task.CompletedTask);

            bool result = await productService.AddProductAsync(userId1, addModel);

            Assert.IsTrue(result);

            mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
            mockProductRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.IsNotNull(capturedProduct);
            Assert.AreEqual("Test Product", capturedProduct.Name);
            Assert.AreEqual("Test Description", capturedProduct.Description);
            Assert.AreEqual(9.99m, capturedProduct.Price);
            Assert.AreEqual("test.jpg", capturedProduct.ImageUrl);
            Assert.IsTrue(capturedProduct.IsActive);
            Assert.AreEqual(categoryId1, capturedProduct.CategoryId);
            Assert.AreEqual(userId1, capturedProduct.CreatorId);
            Assert.IsNotNull(capturedProduct.CreatedAt);
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            IQueryable<Product> mockQueryable = testProducts.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<ProductManagmentIndexViewModel> result = await productService.GetAllProductsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            List<ProductManagmentIndexViewModel> products = result.ToList();
            Assert.AreEqual(productId1, products[0].Id);
            Assert.AreEqual("Organic Honey", products[0].Name);
            Assert.AreEqual("Pure organic honey from mountain flowers", products[0].Description);
            Assert.AreEqual(10.99m, products[0].Price);
            Assert.AreEqual("honey1.jpg", products[0].ImageUrl);
            Assert.AreEqual(categoryId1, products[0].Category.Id);
            Assert.AreEqual("Honey Products", products[0].Category.Name);
            Assert.IsTrue(products[0].IsActive);

            Assert.AreEqual(productId2, products[1].Id);
            Assert.AreEqual("Beeswax Candle", products[1].Name);
            Assert.AreEqual("Handmade beeswax candle", products[1].Description);
            Assert.AreEqual(15.99m, products[1].Price);
            Assert.AreEqual("candle1.jpg", products[1].ImageUrl);
            Assert.AreEqual(categoryId2, products[1].Category.Id);
            Assert.AreEqual("Beeswax Products", products[1].Category.Name);
            Assert.IsTrue(products[1].IsActive);
        }

        [Test]
        public async Task GetProductForDeleteAsync_WithNullId_ReturnsNull()
        {
            DeleteProductManagmentViewModel? result = await productService.GetProductForDeleteAsync(null);

            Assert.IsNull(result);
            mockProductRepository.Verify(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()), Times.Never);
        }

        [Test]
        public async Task GetProductForDeleteAsync_ProductNotFound_ReturnsNull()
        {
            mockProductRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync((Product)null);

            DeleteProductManagmentViewModel? result = await productService.GetProductForDeleteAsync(Guid.NewGuid());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProductForDeleteAsync_ProductFound_ReturnsViewModel()
        {
            mockProductRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(testProducts[0]);

            DeleteProductManagmentViewModel? result = await productService.GetProductForDeleteAsync(productId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(productId1, result.Id);
            Assert.AreEqual("Organic Honey", result.Name);
            Assert.AreEqual("Pure organic honey from mountain flowers", result.Description);
            Assert.AreEqual(10.99m, result.Price);
            Assert.AreEqual("honey1.jpg", result.ImageUrl);
            Assert.IsTrue(result.IsActive);
        }

        [Test]
        public async Task GetProductForDeleteAsync_ProductWithNullDescription_ReturnsEmptyString()
        {
            Product product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "No Description",
                Description = null,
                Price = 5.99m,
                ImageUrl = "nodesc.jpg",
                IsActive = true,
                IsDeleted = false
            };

            mockProductRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);

            DeleteProductManagmentViewModel? result = await productService.GetProductForDeleteAsync(product.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(product.Id, result.Id);
            Assert.AreEqual("No Description", result.Name);
            Assert.AreEqual(string.Empty, result.Description);
            Assert.AreEqual(5.99m, result.Price);
            Assert.AreEqual("nodesc.jpg", result.ImageUrl);
            Assert.IsTrue(result.IsActive);
        }

        [Test]
        public async Task GetProductForEditingAsync_WithNullId_ReturnsNull()
        {
            EditProductManagmentViewModel? result = await productService.GetProductForEditingAsync(null);

            Assert.IsNull(result);
            mockProductRepository.Verify(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()), Times.Never);
        }

        [Test]
        public async Task GetProductForEditingAsync_ProductNotFound_ReturnsNull()
        {
            mockProductRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync((Product)null);

            EditProductManagmentViewModel? result = await productService.GetProductForEditingAsync(Guid.NewGuid());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProductForEditingAsync_ProductFound_ReturnsViewModel()
        {
            mockProductRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(testProducts[0]);

            EditProductManagmentViewModel? result = await productService.GetProductForEditingAsync(productId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(productId1, result.Id);
            Assert.AreEqual("Organic Honey", result.Name);
            Assert.AreEqual("Pure organic honey from mountain flowers", result.Description);
            Assert.AreEqual(10.99m, result.Price);
            Assert.AreEqual("honey1.jpg", result.ImageUrl);
            Assert.IsTrue(result.IsActive);
            Assert.AreEqual(categoryId1, result.CategoryId);
        }

        [Test]
        public async Task PersistUpdateProductAsync_ProductNotFound_ReturnsFalse()
        {
            EditProductManagmentViewModel editModel = new EditProductManagmentViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Updated Name",
                Description = "Updated Description",
                Price = 19.99m,
                ImageUrl = "updated.jpg",
                IsActive = false,
                CategoryId = categoryId1
            };

            mockProductRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync((Product)null);

            bool result = await productService.PersistUpdateProductAsync(editModel);

            Assert.IsFalse(result);
            mockProductRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task PersistUpdateProductAsync_ProductFound_UpdatesProduct()
        {
            EditProductManagmentViewModel editModel = new EditProductManagmentViewModel
            {
                Id = productId1,
                Name = "Updated Honey",
                Description = "Updated Description",
                Price = 19.99m,
                ImageUrl = "updated.jpg",
                IsActive = false,
                CategoryId = categoryId2
            };

            Product product = testProducts[0];
            mockProductRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);

            bool result = await productService.PersistUpdateProductAsync(editModel);

            Assert.IsTrue(result);
            mockProductRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.AreEqual("Updated Honey", product.Name);
            Assert.AreEqual("Updated Description", product.Description);
            Assert.AreEqual(19.99m, product.Price);
            Assert.AreEqual("updated.jpg", product.ImageUrl);
            Assert.IsFalse(product.IsActive);
            Assert.AreEqual(categoryId2, product.CategoryId);
        }

        [Test]
        public async Task SoftDeleteProductAsync_ProductNotFound_ReturnsFalse()
        {
            DeleteProductManagmentViewModel deleteModel = new DeleteProductManagmentViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Product to Delete",
                Description = "Description",
                Price = 10.99m,
                ImageUrl = "image.jpg",
                IsActive = true
            };

            mockProductRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync((Product)null);

            bool result = await productService.SoftDeleteProductAsync(deleteModel);

            Assert.IsFalse(result);
            mockProductRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task SoftDeleteProductAsync_ProductFound_SoftDeletesProduct()
        {
            DeleteProductManagmentViewModel deleteModel = new DeleteProductManagmentViewModel
            {
                Id = productId1,
                Name = "Organic Honey",
                Description = "Pure organic honey from mountain flowers",
                Price = 10.99m,
                ImageUrl = "honey1.jpg",
                IsActive = true
            };

            Product product = testProducts[0];
            mockProductRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);

            bool result = await productService.SoftDeleteProductAsync(deleteModel);

            Assert.IsTrue(result);
            mockProductRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.IsTrue(product.IsDeleted);
        }

        [Test]
        public async Task GetProductStatisticsAsync_CalculatesCorrectStatistics()
        {
            Product outOfStockProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Out of Stock Product",
                Price = 25.99m,
                IsDeleted = false,
                ProductStocks = new List<ProductStock>
                {
                    new ProductStock
                    {
                        Quantity = 0,
                        IsDeleted = false
                    }
                }
            };

            Product lowStockProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Low Stock Product",
                Price = 30.99m,
                IsDeleted = false,
                ProductStocks = new List<ProductStock>
                {
                    new ProductStock
                    {
                        Quantity = 8,
                        IsDeleted = false
                    }
                }
            };

            List<Product> allProducts = testProducts.Concat(new[] { outOfStockProduct, lowStockProduct }).ToList();
            IQueryable<Product> mockQueryable = allProducts.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            ProductStatisticsViewModel stats = await productService.GetProductStatisticsAsync();

            Assert.IsNotNull(stats);

            Assert.AreEqual(4, stats.TotalProducts);

            Assert.AreEqual(2, stats.LowStockProducts);

            Assert.AreEqual(1, stats.OutOfStockProducts);
        }

        [Test]
        public async Task GetProductStatisticsAsync_ExcludesDeletedProducts()
        {
            Product deletedProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Deleted Product",
                Price = 25.99m,
                IsDeleted = true,
                ProductStocks = new List<ProductStock>
                {
                    new ProductStock
                    {
                        Quantity = 0,
                        IsDeleted = false
                    }
                }
            };

            List<Product> allProducts = testProducts.Concat(new[] { deletedProduct }).ToList();
            IQueryable<Product> mockQueryable = allProducts.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            ProductStatisticsViewModel stats = await productService.GetProductStatisticsAsync();

            Assert.IsNotNull(stats);
            Assert.AreEqual(2, stats.TotalProducts);
        }

        [Test]
        public async Task GetProductStatisticsAsync_ExcludesDeletedStocks()
        {
            mockProductRepository = new Mock<IProductRepository>();

            Product productWithDeletedStock = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product with Deleted Stock",
                Price = 25.99m,
                IsDeleted = false,
                ProductStocks = new List<ProductStock>
            {
                new ProductStock
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 0,
                    IsDeleted = true 
                }
            }
            };

            List<Product> products = new List<Product> { productWithDeletedStock };
            IQueryable<Product> mockQueryable = products.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            ProductService productService = new ProductService(
                mockProductRepository.Object,
                mockCategoryRepository.Object,
                mockUserManager.Object,
                mockOrderItemRepository.Object);

            ProductStatisticsViewModel stats = await productService.GetProductStatisticsAsync();

            Assert.IsNotNull(stats);

            Assert.AreEqual(1, stats.TotalProducts);

            Assert.AreEqual(1, stats.OutOfStockProducts);
        }

        [Test]
        public async Task GetBestSellingProductsAsync_ReturnsCorrectProducts()
        {
            IQueryable<OrderItem> mockQueryable = testOrderItems.AsQueryable().BuildMock();

            mockOrderItemRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

           IEnumerable<BestSellingProductViewModel> result = await productService.GetBestSellingProductsAsync(2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            List<BestSellingProductViewModel> products = result.ToList();

            Assert.AreEqual(productId1, products[0].Id);
            Assert.AreEqual("Organic Honey", products[0].Name);
            Assert.AreEqual(5, products[0].TotalSold);
            Assert.AreEqual(54.95m, products[0].TotalRevenue);

            Assert.AreEqual(productId2, products[1].Id);
            Assert.AreEqual("Beeswax Candle", products[1].Name);
            Assert.AreEqual(1, products[1].TotalSold);
            Assert.AreEqual(15.99m, products[1].TotalRevenue);
        }

        [Test]
        public async Task GetBestSellingProductsAsync_WithDefaultCount_ReturnsFiveProducts()
        {
            List<OrderItem> additionalOrderItems = new List<OrderItem>();
            for (int i = 0; i < 5; i++)
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = $"Product {i + 3}",
                    Price = (5 + i) * 1.99m,
                    ImageUrl = $"image{i + 3}.jpg"
                };

                var orderItem = new OrderItem
                {
                    OrderId = Guid.NewGuid(),
                    Order = new Order { IsDeleted = false },
                    ProductId = product.Id,
                    Product = product,
                    Quantity = 5 - i,
                    UnitPrice = product.Price,
                    IsDeleted = false
                };

                additionalOrderItems.Add(orderItem);
            }

            List<OrderItem> allOrderItems = testOrderItems.Concat(additionalOrderItems).ToList();
            IQueryable<OrderItem> mockQueryable = allOrderItems.AsQueryable().BuildMock();

            mockOrderItemRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<BestSellingProductViewModel> result = await productService.GetBestSellingProductsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count());

            List<BestSellingProductViewModel> products = result.ToList();
            for (int i = 0; i < products.Count - 1; i++)
            {
                Assert.IsTrue(products[i].TotalSold >= products[i + 1].TotalSold);
            }
        }

        [Test]
        public async Task GetBestSellingProductsAsync_ExcludesDeletedOrderItems()
        {
            OrderItem deletedOrderItem = new OrderItem
            {
                OrderId = Guid.NewGuid(),
                Order = new Order { IsDeleted = false },
                ProductId = Guid.NewGuid(),
                Product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product with Deleted Order Item",
                    Price = 50.99m,
                    ImageUrl = "deleted-item.jpg"
                },
                Quantity = 20,
                UnitPrice = 50.99m,
                IsDeleted = true
            };

            List<OrderItem> allOrderItems = testOrderItems.Concat(new[] { deletedOrderItem }).ToList();
            IQueryable<OrderItem> mockQueryable = allOrderItems.AsQueryable().BuildMock();

            mockOrderItemRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<BestSellingProductViewModel> result = await productService.GetBestSellingProductsAsync(2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            Assert.IsFalse(result.Any(p => p.Name == "Product with Deleted Order Item"));
        }

        [Test]
        public async Task GetBestSellingProductsAsync_ExcludesOrderItemsFromDeletedOrders()
        {
            OrderItem orderItemFromDeletedOrder = new OrderItem
            {
                OrderId = Guid.NewGuid(),
                Order = new Order { IsDeleted = true },
                ProductId = Guid.NewGuid(),
                Product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product from Deleted Order",
                    Price = 50.99m,
                    ImageUrl = "deleted-order.jpg"
                },
                Quantity = 20,
                UnitPrice = 50.99m,
                IsDeleted = false
            };

            List<OrderItem> allOrderItems = testOrderItems.Concat(new[] { orderItemFromDeletedOrder }).ToList();
            IQueryable<OrderItem> mockQueryable = allOrderItems.AsQueryable().BuildMock();

            mockOrderItemRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<BestSellingProductViewModel> result = await productService.GetBestSellingProductsAsync(2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            Assert.IsFalse(result.Any(p => p.Name == "Product from Deleted Order"));
        }
    }
}