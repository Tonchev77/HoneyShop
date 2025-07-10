namespace HoneyShop.Services.Core.Contracts
{
    using HoneyShop.ViewModels.Home;
    public interface ICategoryService
    {
        Task<IEnumerable<GetAllCategoriesViewModel>> GetAllCategoriesAsync();
    }
}
