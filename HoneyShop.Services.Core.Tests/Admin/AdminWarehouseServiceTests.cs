namespace HoneyShop.Services.Core.Tests.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    [TestFixture]
    public class WarehouseServiceTests
    {
        private Mock<IWarehouseRepository> mockWarehouseRepository;
        private Mock<IProductStockRepository> mockProductStockRepository;
        private Mock<IProductRepository> mockProductRepository;
        private IWarehouseService warehouseService;

        // Test data
        private readonly Guid warehouseId1 = Guid.NewGuid();
        private readonly Guid warehouseId2 = Guid.NewGuid();
        private readonly Guid productId1 = Guid.NewGuid();
        private readonly Guid productId2 = Guid.NewGuid();

        private List<Warehouse> testWarehouses;
        private List<Product> testProducts;
        private List<ProductStock> testProductStocks;

        [SetUp]
        public void Setup()
        {
            mockWarehouseRepository = new Mock<IWarehouseRepository>();
            mockProductStockRepository = new Mock<IProductStockRepository>();
            mockProductRepository = new Mock<IProductRepository>();

            testWarehouses = new List<Warehouse>
            {
                new Warehouse
                {
                    Id = warehouseId1,
                    Name = "Main Warehouse",
                    Location = "Sofia",
                    IsDeleted = false
                },
                new Warehouse
                {
                    Id = warehouseId2,
                    Name = "Secondary Warehouse",
                    Location = "Plovdiv",
                    IsDeleted = false
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
                    IsDeleted = false
                },
                new Product
                {
                    Id = productId2,
                    Name = "Beeswax Candle",
                    Description = "Handmade beeswax candle",
                    Price = 15.99m,
                    ImageUrl = "candle1.jpg",
                    IsActive = true,
                    IsDeleted = false
                }
            };

            testProductStocks = new List<ProductStock>
            {
                new ProductStock
                {
                    WarehouseId = warehouseId1,
                    ProductId = productId1,
                    Quantity = 50,
                    IsDeleted = false
                },
                new ProductStock
                {
                    WarehouseId = warehouseId1,
                    ProductId = productId2,
                    Quantity = 20,
                    IsDeleted = false
                },
                new ProductStock
                {
                    WarehouseId = warehouseId2,
                    ProductId = productId1,
                    Quantity = 30,
                    IsDeleted = false
                }
            };

            warehouseService = new WarehouseService(
                mockWarehouseRepository.Object,
                mockProductStockRepository.Object,
                mockProductRepository.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        [Test]
        public async Task AddWarehouseAsync_WithNullModel_ReturnsFalse()
        {
            bool result = await warehouseService.AddWarehouseAsync(null);

            Assert.IsFalse(result);
            mockWarehouseRepository.Verify(repo => repo.AddAsync(It.IsAny<Warehouse>()), Times.Never);
            mockWarehouseRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task AddWarehouseAsync_WithValidModel_AddsWarehouse()
        {
            AddWarehouseViewModel addModel = new AddWarehouseViewModel
            {
                Name = "Test Warehouse",
                Location = "Test Location"
            };

            Warehouse? capturedWarehouse = null;
            mockWarehouseRepository.Setup(repo => repo.AddAsync(It.IsAny<Warehouse>()))
                .Callback<Warehouse>(w => capturedWarehouse = w)
                .Returns(Task.CompletedTask);

            bool result = await warehouseService.AddWarehouseAsync(addModel);

            Assert.IsTrue(result);

            mockWarehouseRepository.Verify(repo => repo.AddAsync(It.IsAny<Warehouse>()), Times.Once);
            mockWarehouseRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.IsNotNull(capturedWarehouse);
            Assert.AreEqual("Test Warehouse", capturedWarehouse.Name);
            Assert.AreEqual("Test Location", capturedWarehouse.Location);
        }

        [Test]
        public async Task GetAllWarehousesAsync_ReturnsAllWarehouses()
        {
            IQueryable<Warehouse> mockQueryable = testWarehouses.AsQueryable().BuildMock();

            mockWarehouseRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<WarehouseManagmentIndexViewModel> result = await warehouseService.GetAllWarehousesAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            List<WarehouseManagmentIndexViewModel> warehouses = result.ToList();
            Assert.AreEqual(warehouseId1, warehouses[0].Id);
            Assert.AreEqual("Main Warehouse", warehouses[0].Name);
            Assert.AreEqual("Sofia", warehouses[0].Location);

            Assert.AreEqual(warehouseId2, warehouses[1].Id);
            Assert.AreEqual("Secondary Warehouse", warehouses[1].Name);
            Assert.AreEqual("Plovdiv", warehouses[1].Location);
        }

        [Test]
        public async Task GetAllWarehousesAsync_NoWarehouses_ReturnsEmptyList()
        {
            List<Warehouse> emptyList = new List<Warehouse>();
            IQueryable<Warehouse> mockQueryable = emptyList.AsQueryable().BuildMock();

            mockWarehouseRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<WarehouseManagmentIndexViewModel> result = await warehouseService.GetAllWarehousesAsync();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetWarehouseForDeleteAsync_WithNullId_ReturnsNull()
        {
            DeleteWarehouseManagmentViewModel? result = await warehouseService.GetWarehouseForDeleteAsync(null);

            Assert.IsNull(result);
            mockWarehouseRepository.Verify(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Warehouse, bool>>>()), Times.Never);
        }

        [Test]
        public async Task GetWarehouseForDeleteAsync_WarehouseNotFound_ReturnsNull()
        {
            mockWarehouseRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Warehouse, bool>>>()))
                .ReturnsAsync((Warehouse)null);

            DeleteWarehouseManagmentViewModel? result = await warehouseService.GetWarehouseForDeleteAsync(Guid.NewGuid());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetWarehouseForDeleteAsync_WarehouseFound_ReturnsViewModel()
        {
            mockWarehouseRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Warehouse, bool>>>()))
                .ReturnsAsync(testWarehouses[0]);

            DeleteWarehouseManagmentViewModel? result = await warehouseService.GetWarehouseForDeleteAsync(warehouseId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(warehouseId1, result.Id);
            Assert.AreEqual("Main Warehouse", result.Name);
            Assert.AreEqual("Sofia", result.Location);
        }

        [Test]
        public async Task GetWarehouseForEditingAsync_WithNullId_ReturnsNull()
        {
            EditWarehouseManagmentViewModel? result = await warehouseService.GetWarehouseForEditingAsync(null);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetWarehouseForEditingAsync_WarehouseNotFound_ReturnsNull()
        {
            IQueryable<Warehouse> mockQueryable = testWarehouses.AsQueryable().BuildMock();

            mockWarehouseRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            EditWarehouseManagmentViewModel? result = await warehouseService.GetWarehouseForEditingAsync(Guid.NewGuid());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetWarehouseForEditingAsync_WarehouseFound_ReturnsViewModel()
        {
            IQueryable<Warehouse> mockQueryable = testWarehouses.AsQueryable().BuildMock();

            mockWarehouseRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            EditWarehouseManagmentViewModel? result = await warehouseService.GetWarehouseForEditingAsync(warehouseId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(warehouseId1, result.Id);
            Assert.AreEqual("Main Warehouse", result.Name);
            Assert.AreEqual("Sofia", result.Location);
        }

        [Test]
        public async Task PersistUpdateWarehouseAsync_WarehouseNotFound_ReturnsFalse()
        {
            EditWarehouseManagmentViewModel editModel = new EditWarehouseManagmentViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Updated Warehouse",
                Location = "Updated Location"
            };

            mockWarehouseRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Warehouse, bool>>>()))
                .ReturnsAsync((Warehouse)null);

            bool result = await warehouseService.PersistUpdateWarehouseAsync(editModel);

            Assert.IsFalse(result);
            mockWarehouseRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task PersistUpdateWarehouseAsync_WarehouseFound_UpdatesWarehouse()
        {
            EditWarehouseManagmentViewModel editModel = new EditWarehouseManagmentViewModel
            {
                Id = warehouseId1,
                Name = "Updated Warehouse",
                Location = "Updated Location"
            };

            Warehouse warehouse = testWarehouses[0];
            mockWarehouseRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Warehouse, bool>>>()))
                .ReturnsAsync(warehouse);

            bool result = await warehouseService.PersistUpdateWarehouseAsync(editModel);

            Assert.IsTrue(result);
            mockWarehouseRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.AreEqual("Updated Warehouse", warehouse.Name);
            Assert.AreEqual("Updated Location", warehouse.Location);
        }

        [Test]
        public async Task SoftDeleteWarehouseAsync_WarehouseNotFound_ReturnsFalse()
        {
            DeleteWarehouseManagmentViewModel deleteModel = new DeleteWarehouseManagmentViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Warehouse to Delete",
                Location = "Location"
            };

            mockWarehouseRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Warehouse, bool>>>()))
                .ReturnsAsync((Warehouse)null);

            bool result = await warehouseService.SoftDeleteWarehouseAsync(deleteModel);

            Assert.IsFalse(result);
            mockWarehouseRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task SoftDeleteWarehouseAsync_WarehouseFound_SoftDeletesWarehouse()
        {
            DeleteWarehouseManagmentViewModel deleteModel = new DeleteWarehouseManagmentViewModel
            {
                Id = warehouseId1,
                Name = "Main Warehouse",
                Location = "Sofia"
            };

            Warehouse warehouse = testWarehouses[0];
            mockWarehouseRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Warehouse, bool>>>()))
                .ReturnsAsync(warehouse);

            bool result = await warehouseService.SoftDeleteWarehouseAsync(deleteModel);

            Assert.IsTrue(result);
            mockWarehouseRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.IsTrue(warehouse.IsDeleted);
        }

        [Test]
        public async Task AddProductToWarehouseAsync_ProductNotFound_ReturnsFalse()
        {
            AddProductToWarehouseViewModel addModel = new AddProductToWarehouseViewModel
            {
                WarehouseId = warehouseId1,
                ProductId = Guid.NewGuid(),
                Quantity = 10
            };

            mockProductRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync((Product)null);

            bool result = await warehouseService.AddProductToWarehouseAsync(addModel);

            Assert.IsFalse(result);
            mockProductStockRepository.Verify(repo => repo.AddAsync(It.IsAny<ProductStock>()), Times.Never);
            mockProductStockRepository.Verify(repo => repo.Update(It.IsAny<ProductStock>()), Times.Never);
            mockProductStockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task AddProductToWarehouseAsync_ExistingStock_UpdatesQuantity()
        {
            AddProductToWarehouseViewModel addModel = new AddProductToWarehouseViewModel
            {
                WarehouseId = warehouseId1,
                ProductId = productId1,
                Quantity = 10
            };

            mockProductRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(testProducts[0]);

            ProductStock existingStock = testProductStocks[0]; // Current quantity: 50
            mockProductStockRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductStock, bool>>>()))
                .ReturnsAsync(existingStock);

            bool result = await warehouseService.AddProductToWarehouseAsync(addModel);

            Assert.IsTrue(result);

            mockProductStockRepository.Verify(repo => repo.Update(It.IsAny<ProductStock>()), Times.Once);
            mockProductStockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.AreEqual(60, existingStock.Quantity);
        }

        [Test]
        public async Task AddProductToWarehouseAsync_NewStock_CreatesEntry()
        {
            AddProductToWarehouseViewModel addModel = new AddProductToWarehouseViewModel
            {
                WarehouseId = warehouseId2,
                ProductId = productId2,
                Quantity = 15
            };

            mockProductRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(testProducts[1]);

            mockProductStockRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductStock, bool>>>()))
                .ReturnsAsync((ProductStock)null);

            ProductStock capturedStock = null;
            mockProductStockRepository.Setup(repo => repo.AddAsync(It.IsAny<ProductStock>()))
                .Callback<ProductStock>(ps => capturedStock = ps)
                .Returns(Task.CompletedTask);

            bool result = await warehouseService.AddProductToWarehouseAsync(addModel);

            Assert.IsTrue(result);

            mockProductStockRepository.Verify(repo => repo.AddAsync(It.IsAny<ProductStock>()), Times.Once);
            mockProductStockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.IsNotNull(capturedStock);
            Assert.AreEqual(warehouseId2, capturedStock.WarehouseId);
            Assert.AreEqual(productId2, capturedStock.ProductId);
            Assert.AreEqual(15, capturedStock.Quantity);
            Assert.IsFalse(capturedStock.IsDeleted);
        }

        [Test]
        public async Task GetProductsInWarehouseAsync_ReturnsProducts()
        {
            IQueryable<ProductStock> mockProductStockQueryable = testProductStocks
                .Where(ps => ps.WarehouseId == warehouseId1)
                .AsQueryable()
                .BuildMock();

            var mockProductQueryable = testProducts.AsQueryable().BuildMock();

            mockProductStockRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockProductStockQueryable);

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockProductQueryable);

            IEnumerable<GetProductsInWarehouseViewModel> result = await warehouseService.GetProductsInWarehouseAsync(warehouseId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            List<GetProductsInWarehouseViewModel> products = result.ToList();
            Assert.AreEqual(productId1, products[0].ProductId);
            Assert.AreEqual("Organic Honey", products[0].ProductName);
            Assert.AreEqual(50, products[0].Quantity);
            Assert.AreEqual(10.99m, products[0].Price);

            Assert.AreEqual(productId2, products[1].ProductId);
            Assert.AreEqual("Beeswax Candle", products[1].ProductName);
            Assert.AreEqual(20, products[1].Quantity);
            Assert.AreEqual(15.99m, products[1].Price);
        }

        [Test]
        public async Task GetProductsInWarehouseAsync_IncludesDeletedProducts()
        {
            ProductStock deletedStock = new ProductStock
            {
                WarehouseId = warehouseId1,
                ProductId = Guid.NewGuid(),
                Quantity = 5,
                IsDeleted = true
            };

            List<ProductStock> allStocks = testProductStocks.Concat(new[] { deletedStock }).ToList();

            IQueryable<ProductStock> mockProductStockQueryable = allStocks
                .Where(ps => ps.WarehouseId == warehouseId1)
                .AsQueryable()
                .BuildMock();

            Product deletedProduct = new Product
            {
                Id = deletedStock.ProductId,
                Name = "Deleted Product",
                Description = "This product stock is deleted",
                Price = 5.99m,
                ImageUrl = "deleted.jpg",
                IsDeleted = false
            };

            List<Product> allProducts = testProducts.Concat(new[] { deletedProduct }).ToList();
            IQueryable<Product> mockProductQueryable = allProducts.AsQueryable().BuildMock();

            mockProductStockRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockProductStockQueryable);

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockProductQueryable);

            IEnumerable<GetProductsInWarehouseViewModel> result = await warehouseService.GetProductsInWarehouseAsync(warehouseId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());

            var deletedItem = result.FirstOrDefault(p => p.ProductId == deletedStock.ProductId);
            Assert.IsNotNull(deletedItem);
            Assert.IsTrue(deletedItem.IsDeleted);
        }

        [Test]
        public async Task GetProductFromWarehouseForDeleteAsync_WithNullWarehouseId_ReturnsNull()
        {
            DeleteProductFromWarehouseViewModel? result = await warehouseService.GetProductFromWarehouseForDeleteAsync(null, productId1);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProductFromWarehouseForDeleteAsync_ProductNotFound_ReturnsNull()
        {
            IQueryable<ProductStock> mockProductStockQueryable = testProductStocks.AsQueryable().BuildMock();
            IQueryable<Product> mockProductQueryable = testProducts.AsQueryable().BuildMock();

            mockProductStockRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockProductStockQueryable);

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockProductQueryable);

            DeleteProductFromWarehouseViewModel? result = await warehouseService.GetProductFromWarehouseForDeleteAsync(warehouseId1, Guid.NewGuid());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProductFromWarehouseForDeleteAsync_ProductFound_ReturnsViewModel()
        {
            var joinResult = new
            {
                WarehouseId = warehouseId1,
                ProductId = productId1,
                Name = "Organic Honey",
                Description = "Pure organic honey from mountain flowers",
                Quantity = 50,
                ImageUrl = "honey1.jpg",
                Price = 10.99m
            };

            IQueryable<ProductStock> mockProductStockQueryable = testProductStocks.AsQueryable().BuildMock();
            IQueryable<Product> mockProductQueryable = testProducts.AsQueryable().BuildMock();

            mockProductStockRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockProductStockQueryable);

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockProductQueryable);

            DeleteProductFromWarehouseViewModel? result = await warehouseService.GetProductFromWarehouseForDeleteAsync(warehouseId1, productId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(warehouseId1, result.WarehouseId);
            Assert.AreEqual(productId1, result.ProductId);
            Assert.AreEqual("Organic Honey", result.ProductName);
            Assert.AreEqual("Pure organic honey from mountain flowers", result.ProductDescription);
            Assert.AreEqual(50, result.Quantity);
            Assert.AreEqual("honey1.jpg", result.ImageUrl);
            Assert.AreEqual(10.99m, result.Price);
        }

        [Test]
        public async Task GetProductFromWarehouseForEditingAsync_WithNullIds_ReturnsNull()
        {
            EditProductFromWarehouseViewModel? result1 = await warehouseService.GetProductFromWarehouseForEditingAsync(null, productId1);
            EditProductFromWarehouseViewModel? result2 = await warehouseService.GetProductFromWarehouseForEditingAsync(warehouseId1, null);
            EditProductFromWarehouseViewModel? result3 = await warehouseService.GetProductFromWarehouseForEditingAsync(null, null);

            Assert.IsNull(result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
        }

        [Test]
        public async Task GetProductFromWarehouseForEditingAsync_ProductStockNotFound_ReturnsNull()
        {
            mockProductStockRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductStock, bool>>>()))
                .ReturnsAsync((ProductStock)null);

            EditProductFromWarehouseViewModel? result = await warehouseService.GetProductFromWarehouseForEditingAsync(warehouseId1, Guid.NewGuid());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProductFromWarehouseForEditingAsync_ProductStockFound_ReturnsViewModel()
        {
            mockProductStockRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductStock, bool>>>()))
                .ReturnsAsync(testProductStocks[0]);

            IQueryable<Product> mockProductQueryable = testProducts.AsQueryable().BuildMock();
            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockProductQueryable);

            EditProductFromWarehouseViewModel? result = await warehouseService.GetProductFromWarehouseForEditingAsync(warehouseId1, productId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(warehouseId1, result.WarehouseId);
            Assert.AreEqual(productId1, result.ProductId);
            Assert.AreEqual(50, result.Quantity);

            Assert.IsNotNull(result.Products);
            Assert.AreEqual(2, result.Products.Count());
        }

        [Test]
        public async Task PersistUpdateProductFromWarehouseAsync_WithNullModel_ReturnsFalse()
        {
            bool result = await warehouseService.PersistUpdateProductFromWarehouseAsync(null);

            Assert.IsFalse(result);
            mockProductStockRepository.Verify(repo => repo.Update(It.IsAny<ProductStock>()), Times.Never);
            mockProductStockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task PersistUpdateProductFromWarehouseAsync_ProductStockNotFound_ReturnsFalse()
        {
            EditProductFromWarehouseViewModel editModel = new EditProductFromWarehouseViewModel
            {
                WarehouseId = warehouseId1,
                ProductId = productId1,
                Quantity = 75
            };

            mockProductStockRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductStock, bool>>>()))
                .ReturnsAsync((ProductStock)null);

            bool result = await warehouseService.PersistUpdateProductFromWarehouseAsync(editModel);

            Assert.IsFalse(result);
            mockProductStockRepository.Verify(repo => repo.Update(It.IsAny<ProductStock>()), Times.Never);
            mockProductStockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task PersistUpdateProductFromWarehouseAsync_ProductStockFound_UpdatesQuantity()
        {
            EditProductFromWarehouseViewModel editModel = new EditProductFromWarehouseViewModel
            {
                WarehouseId = warehouseId1,
                ProductId = productId1,
                Quantity = 75
            };

            ProductStock productStock = testProductStocks[0];
            mockProductStockRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductStock, bool>>>()))
                .ReturnsAsync(productStock);

            bool result = await warehouseService.PersistUpdateProductFromWarehouseAsync(editModel);

            Assert.IsTrue(result);

            mockProductStockRepository.Verify(repo => repo.Update(It.IsAny<ProductStock>()), Times.Once);
            mockProductStockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.AreEqual(75, productStock.Quantity);
        }

        [Test]
        public async Task SoftDeleteProductFromWarehouseAsync_WithNullModel_ReturnsFalse()
        {
            bool result = await warehouseService.SoftDeleteProductFromWarehouseAsync(null);

            Assert.IsFalse(result);
            mockProductStockRepository.Verify(repo => repo.Update(It.IsAny<ProductStock>()), Times.Never);
            mockProductStockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task SoftDeleteProductFromWarehouseAsync_ProductStockNotFound_ReturnsFalse()
        {
            DeleteProductFromWarehouseViewModel deleteModel = new DeleteProductFromWarehouseViewModel
            {
                WarehouseId = warehouseId1,
                ProductId = productId1
            };

            mockProductStockRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductStock, bool>>>()))
                .ReturnsAsync((ProductStock)null);

            bool result = await warehouseService.SoftDeleteProductFromWarehouseAsync(deleteModel);

            Assert.IsFalse(result);
            mockProductStockRepository.Verify(repo => repo.Update(It.IsAny<ProductStock>()), Times.Never);
            mockProductStockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task SoftDeleteProductFromWarehouseAsync_ProductStockFound_SoftDeletesProductStock()
        {
            DeleteProductFromWarehouseViewModel deleteModel = new DeleteProductFromWarehouseViewModel
            {
                WarehouseId = warehouseId1,
                ProductId = productId1,
                ProductName = "Organic Honey",
                ProductDescription = "Pure organic honey from mountain flowers",
                Quantity = 50,
                ImageUrl = "honey1.jpg",
                Price = 10.99m
            };

            ProductStock productStock = testProductStocks[0];
            mockProductStockRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductStock, bool>>>()))
                .ReturnsAsync(productStock);

            bool result = await warehouseService.SoftDeleteProductFromWarehouseAsync(deleteModel);

            Assert.IsTrue(result);

            mockProductStockRepository.Verify(repo => repo.Update(It.IsAny<ProductStock>()), Times.Once);
            mockProductStockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.IsTrue(productStock.IsDeleted);
        }

        [Test]
        public async Task RecoverProductFromWarehouseAsync_ProductStockNotFound_ReturnsFalse()
        {
            mockProductStockRepository.Setup(repo => repo.GetAllAttached())
                .Returns(testProductStocks.AsQueryable().BuildMock());

            bool result = await warehouseService.RecoverProductFromWarehouseAsync(warehouseId1, Guid.NewGuid());

            Assert.IsFalse(result);
            mockProductStockRepository.Verify(repo => repo.Update(It.IsAny<ProductStock>()), Times.Never);
            mockProductStockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task RecoverProductFromWarehouseAsync_ProductStockFound_RecoversProductStock()
        {
            ProductStock deletedStock = new ProductStock
            {
                WarehouseId = warehouseId1,
                ProductId = Guid.NewGuid(),
                Quantity = 5,
                IsDeleted = true
            };

            List<ProductStock> allStocks = testProductStocks.Concat(new[] { deletedStock }).ToList();
            IQueryable<ProductStock> mockQueryable = allStocks.AsQueryable().BuildMock();

            mockProductStockRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            bool result = await warehouseService.RecoverProductFromWarehouseAsync(warehouseId1, deletedStock.ProductId);

            Assert.IsTrue(result);

            mockProductStockRepository.Verify(repo => repo.Update(It.IsAny<ProductStock>()), Times.Once);
            mockProductStockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.IsFalse(deletedStock.IsDeleted);
        }
    }
}