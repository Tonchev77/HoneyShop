namespace HoneyShop.Services.Core.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.UserManagement;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<UserManagementIndexViewModel?> GetUserForEditingAsync(string userId)
        {
            ApplicationUser? user = await this.userManager
                .Users
                .FirstOrDefaultAsync(u => u.Id.ToLower() == userId.ToLower());

            UserManagementIndexViewModel userForEdit = new UserManagementIndexViewModel();
            if (user != null)
            {
                userForEdit = new UserManagementIndexViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = await this.userManager.GetRolesAsync(user)
                };
            }

            return userForEdit;
        }

        public async Task<IEnumerable<UserManagementIndexViewModel>> GetUserManagementBoardDataAsync(string userId)
        {
            IEnumerable<UserManagementIndexViewModel> users = await this.userManager
                .Users
                .Where(u => u.Id.ToLower() != userId.ToLower())
                .Select(u => new UserManagementIndexViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    Roles = userManager.GetRolesAsync(u)
                        .GetAwaiter()
                        .GetResult()
                })
                .ToArrayAsync();

            return users;
        }

        public async Task<bool> PersistUpdateUserRoleAsync(string userId, string newRoles)
        {
            ApplicationUser? user = await this.userManager
                .Users
                .FirstOrDefaultAsync(u => u.Id.ToLower() == userId.ToLower());

            if (user == null)
            {
                return false;
            }

            IList<string> currentRoles = await this.userManager.GetRolesAsync(user);

            // Remove all current roles
            IdentityResult removeResult = await this.userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return false;
            }

            // Add new roles
            IdentityResult addResult = await this.userManager.AddToRoleAsync(user, newRoles);
            return addResult.Succeeded;
        }
    }
}
