namespace HoneyShop.ViewModels.Admin.UserManagement
{
    using System.ComponentModel.DataAnnotations;
    public class EditUserManagmentViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public IEnumerable<string> Roles { get; set; } = null!;
    }
}
