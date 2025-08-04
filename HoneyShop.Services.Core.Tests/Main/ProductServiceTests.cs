namespace HoneyShop.Services.Core.Tests
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.Services.Core.Tests.Main;
    using HoneyShop.ViewModels.Shop;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> mockProductRepository;
        private Mock<ICategoryRepository> mockCategoryRepository;
        private IProductService productService;

        // Test data
        private readonly Guid productId1 = Guid.NewGuid();
        private readonly Guid productId2 = Guid.NewGuid();
        private readonly Guid categoryId1 = Guid.NewGuid();
        private readonly Guid categoryId2 = Guid.NewGuid();
        private List<Product> testProducts;
        private List<Category> testCategories;

        [SetUp]
        public void Setup()
        {
            mockProductRepository = new Mock<IProductRepository>();
            mockCategoryRepository = new Mock<ICategoryRepository>();

            testCategories = new List<Category>
            {
                new Category
                {
                    Id = categoryId1,
                    Name = "Honey",
                    IsDeleted = false
                },
                new Category
                {
                    Id = categoryId2,
                    Name = "Beeswax Products",
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
                    CategoryId = categoryId1,
                    Category = testCategories[0],
                    CreatedAt = DateTime.Parse("2025-08-02 18:42:36"), // 2 days ago
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
                    CategoryId = categoryId2,
                    Category = testCategories[1],
                    CreatedAt = DateTime.Parse("2025-08-03 18:42:36"), // 1 day ago
                    IsDeleted = false
                }
            };

            productService = new ProductService(
                mockProductRepository.Object,
                mockCategoryRepository.Object);
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            IQueryable<Product> mockQueryable = testProducts.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<GetAllProductsViewModel> result = await productService.GetAllProductsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            List<GetAllProductsViewModel> products = result.ToList();
            Assert.AreEqual(productId1, products[0].Id);
            Assert.AreEqual("Organic Honey", products[0].Name);
            Assert.AreEqual(10.99m, products[0].Price);
            Assert.AreEqual("honey1.jpg", products[0].ImageUrl);
            Assert.AreEqual(categoryId1, products[0].Category.Id);
            Assert.AreEqual("Honey", products[0].Category.Name);

            Assert.AreEqual(productId2, products[1].Id);
            Assert.AreEqual("Beeswax Candle", products[1].Name);
            Assert.AreEqual(15.99m, products[1].Price);
            Assert.AreEqual("candle1.jpg", products[1].ImageUrl);
            Assert.AreEqual(categoryId2, products[1].Category.Id);
            Assert.AreEqual("Beeswax Products", products[1].Category.Name);
        }

        [Test]
        public async Task GetAllProductsByCategoryAsync_WithNullCategoryId_ReturnsAllProducts()
        {
            IQueryable<Product> mockQueryable = testProducts.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<GetAllProductsViewModel> result = await productService.GetAllProductsByCategoryAsync(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllProductsByCategoryAsync_WithCategoryId_ReturnsFilteredProducts()
        {
            List<Product> filteredProducts = testProducts
                .Where(p => p.CategoryId == categoryId1)
                .ToList();

            IQueryable<Product> mockQueryable = filteredProducts.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<GetAllProductsViewModel> result = await productService.GetAllProductsByCategoryAsync(categoryId1);


            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            GetAllProductsViewModel product = result.First();
            Assert.AreEqual(productId1, product.Id);
            Assert.AreEqual("Organic Honey", product.Name);
            Assert.AreEqual(categoryId1, product.Category.Id);
        }

        [Test]
        public async Task GetAllProductsByStringAndCategoryAsync_ReturnsMatchingProducts()
        {

            List<Product> filteredProducts = testProducts
                .Where(p => (p.Name.ToLower().Contains("organic") ||
                            p.Description.ToLower().Contains("organic")) &&
                            p.CategoryId == categoryId1)
                .ToList();

            IQueryable<Product> mockQueryable = filteredProducts.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<GetAllProductsViewModel> result = await productService.GetAllProductsByStringAndCategoryAsync("Organic", categoryId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            GetAllProductsViewModel product = result.First();
            Assert.AreEqual(productId1, product.Id);
            Assert.AreEqual("Organic Honey", product.Name);
        }

        [Test]
        public async Task GetAllProductsByStringAndCategoryAsync_SearchInDescription_ReturnsMatchingProducts()
        {
            List<Product> filteredProducts = testProducts
                .Where(p => (p.Name.ToLower().Contains("mountain") ||
                            p.Description.ToLower().Contains("mountain")) &&
                            p.CategoryId == categoryId1)
                .ToList();

            IQueryable<Product> mockQueryable = filteredProducts.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<GetAllProductsViewModel> result = await productService.GetAllProductsByStringAndCategoryAsync("mountain", categoryId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            GetAllProductsViewModel product = result.First();
            Assert.AreEqual(productId1, product.Id);
            Assert.AreEqual("Organic Honey", product.Name);
        }

        [Test]
        public async Task GetAllProductsByStringAndCategoryAsync_NoMatchingProducts_ReturnsEmptyList()
        {

            List<Product> emptyList = new List<Product>();
            IQueryable<Product> mockQueryable = emptyList.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<GetAllProductsViewModel> result = await productService.GetAllProductsByStringAndCategoryAsync("notfound", categoryId1);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllProductsByStringAsync_WithNullSearchString_ReturnsAllProducts()
        {
            IQueryable<Product> mockQueryable = testProducts.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<GetAllProductsViewModel> result = await productService.GetAllProductsByStringAsync(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllProductsByStringAsync_WithEmptySearchString_ReturnsAllProducts()
        {
            IQueryable<Product> mockQueryable = testProducts.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<GetAllProductsViewModel> result = await productService.GetAllProductsByStringAsync("");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAllProductsByStringAsync_WithSearchString_ReturnsMatchingProducts()
        {

            List<Product> filteredProducts = testProducts
                .Where(p => p.Name.ToLower().Contains("candle") ||
                            p.Description.ToLower().Contains("candle"))
                .ToList();

            IQueryable<Product> mockQueryable = filteredProducts.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<GetAllProductsViewModel> result = await productService.GetAllProductsByStringAsync("candle");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            GetAllProductsViewModel product = result.First();
            Assert.AreEqual(productId2, product.Id);
            Assert.AreEqual("Beeswax Candle", product.Name);
        }

        [Test]
        public async Task GetProductDetailAsync_WithNullId_ReturnsNull()
        {
          GetProductDetailViewModel? result = await productService.GetProductDetailAsync(null);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProductDetailAsync_ProductNotFound_ReturnsNull()
        {
            mockProductRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync((Product)null);

            GetProductDetailViewModel? result = await productService.GetProductDetailAsync(Guid.NewGuid());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProductDetailAsync_CategoryNotFound_ReturnsNull()
        {
            mockProductRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(testProducts[0]);

            mockCategoryRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Category)null);


            GetProductDetailViewModel? result = await productService.GetProductDetailAsync(productId1);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProductDetailAsync_ValidId_ReturnsProductDetails()
        {
            mockProductRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(testProducts[0]);

            mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId1))
                .ReturnsAsync(testCategories[0]);

            GetProductDetailViewModel? result = await productService.GetProductDetailAsync(productId1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(productId1, result.Id);
            Assert.AreEqual("Organic Honey", result.Name);
            Assert.AreEqual("Pure organic honey from mountain flowers", result.Description);
            Assert.AreEqual(10.99m, result.Price);
            Assert.AreEqual("honey1.jpg", result.ImageUrl);
            Assert.IsTrue(result.IsActive);
            Assert.AreEqual(categoryId1, result.Category.Id);
            Assert.AreEqual("Honey", result.Category.Name);
        }

        [Test]
        public async Task GetProductsForHomeIndexAsync_ReturnsLatestFourProducts()
        {

            List<Product> additionalProducts = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 3",
                    CategoryId = categoryId1,
                    Category = testCategories[0],
                    CreatedAt = DateTime.Parse("2025-07-31 18:42:36"),
                    IsDeleted = false
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 4",
                    CategoryId = categoryId1,
                    Category = testCategories[0],
                    CreatedAt = DateTime.Parse("2025-08-01 18:42:36"),
                    IsDeleted = false
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "New Product",
                    CategoryId = categoryId1,
                    Category = testCategories[0],
                    CreatedAt = DateTime.Parse("2025-08-04 12:42:36"),
                    IsDeleted = false
                }
            };

            List<Product> allProducts = testProducts.Concat(additionalProducts).ToList();

            List<Product> topFourProducts = allProducts
                .OrderByDescending(p => p.CreatedAt)
                .Take(4)
                .ToList();

            IQueryable<Product> mockQueryable = topFourProducts.AsQueryable().BuildMock();

            mockProductRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<GetAllProductsViewModel> result = await productService.GetProductsForHomeIndexAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());

            List<GetAllProductsViewModel> products = result.ToList();
            Assert.AreEqual("New Product", products[0].Name);
            Assert.AreEqual("Beeswax Candle", products[1].Name);
            Assert.AreEqual("Organic Honey", products[2].Name);
            Assert.AreEqual("Product 4", products[3].Name);
        }
    }
}