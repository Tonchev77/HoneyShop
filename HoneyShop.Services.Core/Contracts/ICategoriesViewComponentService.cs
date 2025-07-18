using HoneyShop.ViewModels.Home;

namespace HoneyShop.Services.Core.Contracts
{
    public interface ICategoriesViewComponentService
    {
        Task<IEnumerable<GetAllCategoriesViewModel>> GetAllCategoriesAsync();
    }
}
