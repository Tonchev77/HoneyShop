namespace HoneyShop.Services.Core.Admin.Contracts
{
    using HoneyShop.ViewModels.Admin.UserManagement;
    public interface IUserService
    {
        Task<IEnumerable<UserManagementIndexViewModel>> GetUserManagementBoardDataAsync(string userId);

        Task<UserManagementIndexViewModel?> GetUserForEditingAsync(string userId);

        Task<bool> PersistUpdateUserRoleAsync(string userId, string newRoles);

        //TODO
        Task<bool> SoftDeleteUserAsync(string userId, UserManagementIndexViewModel inputModel);
    }
}
