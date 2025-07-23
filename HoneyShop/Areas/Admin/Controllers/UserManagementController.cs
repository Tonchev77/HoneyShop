namespace HoneyShop.Areas.Admin.Controllers
{
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.UserManagement;
    using Microsoft.AspNetCore.Mvc;

    public class UserManagementController : BaseAdminController
    {
        private readonly IUserService userService;

        public UserManagementController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserManagementIndexViewModel> allUsers = await this.userService
                .GetUserManagementBoardDataAsync(this.GetUserId()!);

            return View(allUsers);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return RedirectToAction(nameof(Index));
                }
                UserManagementIndexViewModel? userForEdit = await this.userService
                    .GetUserForEditingAsync(id);

                if (userForEdit == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(userForEdit);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View(nameof(Index), "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string newRoles)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(newRoles))
                {
                    return RedirectToAction(nameof(Edit));
                }
                bool isUpdated = await this.userService
                    .PersistUpdateUserRoleAsync(id, newRoles);
                if (!isUpdated)
                {
                    return RedirectToAction(nameof(Edit));
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View(nameof(Index), "Home");
            }
        }
    }
}
