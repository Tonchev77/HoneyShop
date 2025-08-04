namespace HoneyShop.Services.Core.Tests.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.CategoryManagment;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    [TestFixture]
    public class AdminCategoryServiceTests
    {
        private Mock<ICategoryRepository> mockCategoryRepository;
        private ICategoryService categoryService;

        // Test data
        private readonly Guid categoryId1 = Guid.NewGuid();
        private readonly Guid categoryId2 = Guid.NewGuid();
        private List<Category> testCategories;

        [SetUp]
        public void Setup()
        {
            mockCategoryRepository = new Mock<ICategoryRepository>();

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

            categoryService = new CategoryService(mockCategoryRepository.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        [Test]
        public async Task AddCategoryAsync_WithNullModel_ReturnsFalse()
        {

            bool result = await categoryService.AddCategoryAsync(null);

            Assert.IsFalse(result);
            mockCategoryRepository.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Never);
            mockCategoryRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task AddCategoryAsync_WithValidModel_AddsCategory()
        {
            AddCategoryViewModel addModel = new AddCategoryViewModel
            {
                Name = "Test Category",
                Description = "Test Description"
            };

            Category? capturedCategory = null;
            mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
                .Callback<Category>(c => capturedCategory = c)
                .Returns(Task.CompletedTask);

            bool result = await categoryService.AddCategoryAsync(addModel);

            Assert.IsTrue(result);

            mockCategoryRepository.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Once);
            mockCategoryRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.IsNotNull(capturedCategory);
            Assert.AreEqual("Test Category", capturedCategory.Name);
            Assert.AreEqual("Test Description", capturedCategory.Description);
        }

        [Test]
        public async Task GetCategoryForDeleteAsync_WithNullId_ReturnsNull()
        {
            DeleteCategoryManagmentViewModel? result = await categoryService.GetCategoryForDeleteAsync(null);

            Assert.IsNull(result);
            mockCategoryRepository.Verify(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>()), Times.Never);
        }

        [Test]
        public async Task GetCategoryForDeleteAsync_CategoryNotFound_ReturnsNull()
        {
            mockCategoryRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Category)null);

            DeleteCategoryManagmentViewModel? result = await categoryService.GetCategoryForDeleteAsync(Guid.NewGuid());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetCategoryForDeleteAsync_CategoryFound_ReturnsViewModel()
        {
            mockCategoryRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(testCategories[0]);

            DeleteCategoryManagmentViewModel? result = await categoryService.GetCategoryForDeleteAsync(categoryId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(categoryId1, result.Id);
            Assert.AreEqual("Honey Products", result.Name);
            Assert.AreEqual("All types of honey products", result.Description);
        }

        [Test]
        public async Task GetCategoryForDeleteAsync_CategoryWithNullDescription_ReturnsEmptyString()
        {
            Category category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "No Description",
                Description = null,
                IsDeleted = false
            };

            mockCategoryRepository.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(category);

            DeleteCategoryManagmentViewModel? result = await categoryService.GetCategoryForDeleteAsync(category.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(category.Id, result.Id);
            Assert.AreEqual("No Description", result.Name);
            Assert.AreEqual(string.Empty, result.Description);
        }

        [Test]
        public async Task GetAllCategoriesAsync_ReturnsAllCategories()
        {
            IQueryable<Category> mockQueryable = testCategories.AsQueryable().BuildMock();

            mockCategoryRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

           IEnumerable<CategoryManagmentIndexViewModel> result = await categoryService.GetAllCategoriesAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            List<CategoryManagmentIndexViewModel> categories = result.ToList();
            Assert.AreEqual(categoryId1, categories[0].Id);
            Assert.AreEqual("Honey Products", categories[0].Name);
            Assert.AreEqual("All types of honey products", categories[0].Description);

            Assert.AreEqual(categoryId2, categories[1].Id);
            Assert.AreEqual("Beeswax Products", categories[1].Name);
            Assert.AreEqual("Products made from beeswax", categories[1].Description);
        }

        [Test]
        public async Task GetAllCategoriesAsync_NoCategories_ReturnsEmptyList()
        {
            List<Category> emptyList = new List<Category>();
            IQueryable<Category> mockQueryable = emptyList.AsQueryable().BuildMock();

            mockCategoryRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<CategoryManagmentIndexViewModel> result = await categoryService.GetAllCategoriesAsync();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetCategoryForEditingAsync_WithNullId_ReturnsNull()
        {
            EditCategoryManagmentViewModel? result = await categoryService.GetCategoryForEditingAsync(null);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetCategoryForEditingAsync_CategoryNotFound_ReturnsNull()
        {
            IQueryable<Category> mockQueryable = testCategories.AsQueryable().BuildMock();

            mockCategoryRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            EditCategoryManagmentViewModel? result = await categoryService.GetCategoryForEditingAsync(Guid.NewGuid());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetCategoryForEditingAsync_CategoryFound_ReturnsViewModel()
        {
            IQueryable<Category> mockQueryable = testCategories.AsQueryable().BuildMock();

            mockCategoryRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            EditCategoryManagmentViewModel? result = await categoryService.GetCategoryForEditingAsync(categoryId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(categoryId1, result.Id);
            Assert.AreEqual("Honey Products", result.Name);
            Assert.AreEqual("All types of honey products", result.Description);
        }

        [Test]
        public async Task GetCategoryForEditingAsync_CategoryWithNullDescription_ReturnsEmptyString()
        {
            Category category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "No Description",
                Description = null,
                IsDeleted = false
            };

            List<Category> testList = new List<Category> { category };
            IQueryable<Category> mockQueryable = testList.AsQueryable().BuildMock();

            mockCategoryRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            EditCategoryManagmentViewModel? result = await categoryService.GetCategoryForEditingAsync(category.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(category.Id, result.Id);
            Assert.AreEqual("No Description", result.Name);
            Assert.AreEqual(string.Empty, result.Description);
        }

        [Test]
        public async Task PersistUpdateCategoryAsync_CategoryNotFound_ReturnsFalse()
        {
            EditCategoryManagmentViewModel editModel = new EditCategoryManagmentViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Updated Name",
                Description = "Updated Description"
            };

            mockCategoryRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Category)null);

            bool result = await categoryService.PersistUpdateCategoryAsync(editModel);

            Assert.IsFalse(result);
            mockCategoryRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task PersistUpdateCategoryAsync_CategoryFound_UpdatesCategory()
        {
            EditCategoryManagmentViewModel editModel = new EditCategoryManagmentViewModel
            {
                Id = categoryId1,
                Name = "Updated Name",
                Description = "Updated Description"
            };

            Category category = testCategories[0];
            mockCategoryRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(category);

            bool result = await categoryService.PersistUpdateCategoryAsync(editModel);

            Assert.IsTrue(result);
            mockCategoryRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.AreEqual("Updated Name", category.Name);
            Assert.AreEqual("Updated Description", category.Description);
        }

        [Test]
        public async Task SoftDeleteCategoryAsync_CategoryNotFound_ReturnsFalse()
        {
            DeleteCategoryManagmentViewModel deleteModel = new DeleteCategoryManagmentViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Category to Delete",
                Description = "Description"
            };

            mockCategoryRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Category)null);

            bool result = await categoryService.SoftDeleteCategoryAsync(deleteModel);

            Assert.IsFalse(result);
            mockCategoryRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task SoftDeleteCategoryAsync_CategoryFound_SoftDeletesCategory()
        {
            DeleteCategoryManagmentViewModel deleteModel = new DeleteCategoryManagmentViewModel
            {
                Id = categoryId1,
                Name = "Honey Products",
                Description = "All types of honey products"
            };

            Category category = testCategories[0];
            mockCategoryRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(category);

            bool result = await categoryService.SoftDeleteCategoryAsync(deleteModel);

            Assert.IsTrue(result);
            mockCategoryRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.IsTrue(category.IsDeleted);
        }

        [Test]
        public async Task GetCategoryDropdownDataAsync_ReturnsAllCategories()
        {
            IQueryable<Category> mockQueryable = testCategories.AsQueryable().BuildMock();

            mockCategoryRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<AddProductCategoryDropDownModel> result = await categoryService.GetCategoryDropdownDataAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            List<AddProductCategoryDropDownModel> categories = result.ToList();
            Assert.AreEqual(categoryId1, categories[0].Id);
            Assert.AreEqual("Honey Products", categories[0].Name);

            Assert.AreEqual(categoryId2, categories[1].Id);
            Assert.AreEqual("Beeswax Products", categories[1].Name);
        }

        [Test]
        public async Task GetCategoryDropdownDataAsync_NoCategories_ReturnsEmptyList()
        {
            List<Category> emptyList = new List<Category>();
            IQueryable<Category> mockQueryable = emptyList.AsQueryable().BuildMock();

            mockCategoryRepository.Setup(repo => repo.GetAllAttached())
                .Returns(mockQueryable);

            IEnumerable<AddProductCategoryDropDownModel> result = await categoryService.GetCategoryDropdownDataAsync();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }
    }
}
