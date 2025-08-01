namespace HoneyShop.Services.Core.Tests.Main
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Home;
    using MockQueryable;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> categoryRepositoryMock;
        private ICategoryService categoryService;

        [SetUp]
        public void Setup()
        {
            this.categoryRepositoryMock = new Mock<ICategoryRepository>();
            this.categoryService = new CategoryService(this.categoryRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        [Test]
        public async Task GetAllCategoriesAsync_ShouldReturnAllCategories()
        {
            List<Category> categoryList = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Honey" },
                new Category { Id = Guid.NewGuid(), Name = "Wax" },
                new Category { Id = Guid.NewGuid(), Name = "Propolis" }
            };

            IQueryable<Category> mockQueryable = categoryList.BuildMock();

            this.categoryRepositoryMock
                .Setup(x => x.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<GetAllCategoriesViewModel> result = await this.categoryService.GetAllCategoriesAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result.Any(c => c.Name == "Honey"), Is.True);
            Assert.That(result.Any(c => c.Name == "Wax"), Is.True);
            Assert.That(result.Any(c => c.Name == "Propolis"), Is.True);

            this.categoryRepositoryMock.Verify(x => x.GetAllAttached(), Times.Once);
        }

        [Test]
        public async Task GetAllCategoriesAsync_WithNoCategories_ShouldReturnEmptyCollection()
        {
            IQueryable<Category> emptyList = new List<Category>().BuildMock();

            this.categoryRepositoryMock
                .Setup(x => x.GetAllAttached())
                .Returns(emptyList);

            IEnumerable<GetAllCategoriesViewModel> result = await this.categoryService.GetAllCategoriesAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            this.categoryRepositoryMock.Verify(x => x.GetAllAttached(), Times.Once);
        }

        [Test]
        public void GetAllCategoriesAsync_WithRepositoryException_ShouldPropagateException()
        {
            this.categoryRepositoryMock
                .Setup(x => x.GetAllAttached())
                .Throws(new Exception("Database error"));

            Assert.ThrowsAsync<Exception>(async () => await this.categoryService.GetAllCategoriesAsync());

            this.categoryRepositoryMock.Verify(x => x.GetAllAttached(), Times.Once);
        }
    }
}
