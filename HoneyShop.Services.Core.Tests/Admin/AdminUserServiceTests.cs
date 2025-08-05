namespace HoneyShop.Services.Core.Tests.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Services.Core.Admin;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.Home;
    using HoneyShop.ViewModels.Admin.UserManagement;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class AdminUserServiceTests
    {
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private Mock<RoleManager<IdentityRole>> mockRoleManager;
        private IUserService userService;

        private readonly string userId1 = Guid.NewGuid().ToString();
        private readonly string userId2 = Guid.NewGuid().ToString();
        private readonly string currentUserId = Guid.NewGuid().ToString();
        private readonly DateTime currentDate = DateTime.Parse("2025-08-05 17:49:46");
        private List<ApplicationUser> testUsers;

        [SetUp]
        public void Setup()
        {
            Mock<IUserStore<ApplicationUser>> userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            Mock<IRoleStore<IdentityRole>> roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object, null, null, null, null);

            testUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = userId1,
                    UserName = "user1",
                    Email = "user1@example.com",
                    CreatedAt = currentDate.AddDays(-10) // 10 days ago
                },
                new ApplicationUser
                {
                    Id = userId2,
                    UserName = "user2",
                    Email = "user2@example.com",
                    CreatedAt = currentDate.AddDays(-50) // 50 days ago
                },
                new ApplicationUser
                {
                    Id = currentUserId,
                    UserName = "Tonchev77",
                    Email = "admin@honeyshop.com",
                    CreatedAt = currentDate.AddYears(-1) // 1 year ago
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "newuser",
                    Email = "newuser@example.com",
                    CreatedAt = currentDate.AddDays(-1) // 1 day ago (this month)
                }
            };

            userService = new UserService(mockUserManager.Object, mockRoleManager.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        [Test]
        public async Task GetUserForEditingAsync_UserFound_ReturnsViewModel()
        {
            ApplicationUser? user = testUsers[0];

            mockUserManager.Setup(um => um.Users)
                .Returns(testUsers.AsQueryable().BuildMock());

            mockUserManager.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == userId1)))
                .ReturnsAsync(new List<string> { "User" });

            UserManagementIndexViewModel? result = await userService.GetUserForEditingAsync(userId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(userId1, result.Id);
            Assert.AreEqual("user1@example.com", result.Email);
            Assert.AreEqual(1, result.Roles.Count());
            Assert.AreEqual("User", result.Roles.First());
        }

        [Test]
        public async Task GetUserForEditingAsync_UserNotFound_ReturnsEmptyViewModel()
        {
            mockUserManager.Setup(um => um.Users)
                .Returns(testUsers.AsQueryable().BuildMock());

            UserManagementIndexViewModel? result = await userService.GetUserForEditingAsync(Guid.NewGuid().ToString());

            Assert.IsNotNull(result);
            Assert.IsNull(result.Id);
            Assert.IsNull(result.Email);
            Assert.IsNull(result.Roles);
        }

        [Test]
        public async Task GetUserForEditingAsync_UserWithMultipleRoles_ReturnsAllRoles()
        {
            ApplicationUser? user = testUsers[0];

            mockUserManager.Setup(um => um.Users)
                .Returns(testUsers.AsQueryable().BuildMock());

            mockUserManager.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == userId1)))
                .ReturnsAsync(new List<string> { "User", "Admin", "Editor" });

            UserManagementIndexViewModel? result = await userService.GetUserForEditingAsync(userId1);

            Assert.IsNotNull(result);
            Assert.AreEqual(userId1, result.Id);
            Assert.AreEqual("user1@example.com", result.Email);
            Assert.AreEqual(3, result.Roles.Count());
            CollectionAssert.Contains(result.Roles, "User");
            CollectionAssert.Contains(result.Roles, "Admin");
            CollectionAssert.Contains(result.Roles, "Editor");
        }

        [Test]
        public async Task GetUserManagementBoardDataAsync_ExcludesCurrentUser_ReturnsOtherUsers()
        {
            mockUserManager.Setup(um => um.Users)
                .Returns(testUsers.AsQueryable().BuildMock());

            mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "User" });

            IEnumerable<UserManagementIndexViewModel> result = await userService.GetUserManagementBoardDataAsync(currentUserId);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());

            Assert.IsFalse(result.Any(u => u.Id == currentUserId));

            Assert.IsTrue(result.Any(u => u.Id == userId1));
            Assert.IsTrue(result.Any(u => u.Id == userId2));
        }

        [Test]
        public async Task GetUserManagementBoardDataAsync_IncludesUserRoles()
        {
            mockUserManager.Setup(um => um.Users)
                .Returns(testUsers.AsQueryable().BuildMock());

            mockUserManager.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == userId1)))
                .ReturnsAsync(new List<string> { "User" });

            mockUserManager.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == userId2)))
                .ReturnsAsync(new List<string> { "Admin" });

            IEnumerable<UserManagementIndexViewModel> result = await userService.GetUserManagementBoardDataAsync(currentUserId);

            Assert.IsNotNull(result);

            UserManagementIndexViewModel? user1 = result.FirstOrDefault(u => u.Id == userId1);
            Assert.IsNotNull(user1);
            Assert.AreEqual(1, user1.Roles.Count());
            Assert.AreEqual("User", user1.Roles.First());

            UserManagementIndexViewModel? user2 = result.FirstOrDefault(u => u.Id == userId2);
            Assert.IsNotNull(user2);
            Assert.AreEqual(1, user2.Roles.Count());
            Assert.AreEqual("Admin", user2.Roles.First());
        }

        [Test]
        public async Task GetUserManagementBoardDataAsync_CaseInsensitiveUserIdComparison()
        {
            mockUserManager.Setup(um => um.Users)
                .Returns(testUsers.AsQueryable().BuildMock());

            mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "User" });

            IEnumerable<UserManagementIndexViewModel> result = await userService.GetUserManagementBoardDataAsync(currentUserId.ToLower());

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());

            Assert.IsFalse(result.Any(u => u.Id.ToLower() == currentUserId.ToLower()));
        }

        [Test]
        public async Task PersistUpdateUserRoleAsync_UserNotFound_ReturnsFalse()
        {
            mockUserManager.Setup(um => um.Users)
                .Returns(testUsers.AsQueryable().BuildMock());

            bool result = await userService.PersistUpdateUserRoleAsync(Guid.NewGuid().ToString(), "Admin");

            Assert.IsFalse(result);
            mockUserManager.Verify(um => um.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()), Times.Never);
            mockUserManager.Verify(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task PersistUpdateUserRoleAsync_RoleDoesNotExist_ReturnsFalse()
        {
            ApplicationUser? user = testUsers[0];

            mockUserManager.Setup(um => um.Users)
                .Returns(testUsers.AsQueryable().BuildMock());

            mockRoleManager.Setup(rm => rm.RoleExistsAsync("NonExistentRole"))
                .ReturnsAsync(false);


            bool result = await userService.PersistUpdateUserRoleAsync(userId1, "NonExistentRole");

            Assert.IsFalse(result);
            mockUserManager.Verify(um => um.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()), Times.Never);
            mockUserManager.Verify(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task PersistUpdateUserRoleAsync_RemoveRolesFails_ReturnsFalse()
        {
            ApplicationUser? user = testUsers[0];

            mockUserManager.Setup(um => um.Users)
                .Returns(testUsers.AsQueryable().BuildMock());

            mockRoleManager.Setup(rm => rm.RoleExistsAsync("Admin"))
                .ReturnsAsync(true);

            mockUserManager.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == userId1)))
                .ReturnsAsync(new List<string> { "User" });

            mockUserManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to remove roles" }));

            bool result = await userService.PersistUpdateUserRoleAsync(userId1, "Admin");

            Assert.IsFalse(result);
            mockUserManager.Verify(um => um.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()), Times.Once);
            mockUserManager.Verify(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task PersistUpdateUserRoleAsync_AddRoleFails_ReturnsFalse()
        {
            ApplicationUser? user = testUsers[0];

            mockUserManager.Setup(um => um.Users)
                .Returns(testUsers.AsQueryable().BuildMock());

            mockRoleManager.Setup(rm => rm.RoleExistsAsync("Admin"))
                .ReturnsAsync(true);

            mockUserManager.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == userId1)))
                .ReturnsAsync(new List<string> { "User" });

            mockUserManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to add role" }));

            bool result = await userService.PersistUpdateUserRoleAsync(userId1, "Admin");

            Assert.IsFalse(result);
            mockUserManager.Verify(um => um.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()), Times.Once);
            mockUserManager.Verify(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task PersistUpdateUserRoleAsync_Success_ReturnsTrue()
        {
            ApplicationUser? user = testUsers[0];

            mockUserManager.Setup(um => um.Users)
                .Returns(testUsers.AsQueryable().BuildMock());

            mockRoleManager.Setup(rm => rm.RoleExistsAsync("Admin"))
                .ReturnsAsync(true);

            mockUserManager.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == userId1)))
                .ReturnsAsync(new List<string> { "User" });

            mockUserManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            bool result = await userService.PersistUpdateUserRoleAsync(userId1, "Admin");

            Assert.IsTrue(result);
            mockUserManager.Verify(um => um.RemoveFromRolesAsync(It.Is<ApplicationUser>(u => u.Id == userId1), It.Is<IEnumerable<string>>(r => r.Contains("User"))), Times.Once);
            mockUserManager.Verify(um => um.AddToRoleAsync(It.Is<ApplicationUser>(u => u.Id == userId1), "Admin"), Times.Once);
        }

        [Test]
        public async Task PersistUpdateUserRoleAsync_CaseInsensitiveUserIdComparison()
        {
            ApplicationUser? user = testUsers[0];

            mockUserManager.Setup(um => um.Users)
                .Returns(testUsers.AsQueryable().BuildMock());

            mockRoleManager.Setup(rm => rm.RoleExistsAsync("Admin"))
                .ReturnsAsync(true);

            mockUserManager.Setup(um => um.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == userId1)))
                .ReturnsAsync(new List<string> { "User" });

            mockUserManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            bool result = await userService.PersistUpdateUserRoleAsync(userId1.ToUpper(), "Admin");

            Assert.IsTrue(result);
            mockUserManager.Verify(um => um.RemoveFromRolesAsync(It.Is<ApplicationUser>(u => u.Id == userId1), It.Is<IEnumerable<string>>(r => r.Contains("User"))), Times.Once);
            mockUserManager.Verify(um => um.AddToRoleAsync(It.Is<ApplicationUser>(u => u.Id == userId1), "Admin"), Times.Once);
        }

        [Test]
        public async Task GetCustomerStatisticsAsync_CalculatesCorrectStatistics()
        {
            DateTime currentDate = this.currentDate;
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

            List<ApplicationUser> allUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = firstDayOfMonth.AddDays(-10) // Last month
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = firstDayOfMonth // First day of current month
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = currentDate.AddDays(-1) // Yesterday, current month
                }
            };

            mockUserManager.Setup(um => um.Users)
                .Returns(allUsers.AsQueryable().BuildMock());

            CustomerStatisticsViewModel stats = await userService.GetCustomerStatisticsAsync();

            Assert.IsNotNull(stats);

            Assert.AreEqual(3, stats.TotalCustomers);

            Assert.AreEqual(2, stats.NewCustomersThisMonth);
        }

        [Test]
        public async Task GetCustomerStatisticsAsync_WithNoUsers_ReturnsZeroStats()
        {
            mockUserManager.Setup(um => um.Users)
                .Returns(new List<ApplicationUser>().AsQueryable().BuildMock());

            CustomerStatisticsViewModel stats = await userService.GetCustomerStatisticsAsync();

            Assert.IsNotNull(stats);
            Assert.AreEqual(0, stats.TotalCustomers);
            Assert.AreEqual(0, stats.NewCustomersThisMonth);
        }
    }
}