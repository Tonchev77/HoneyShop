namespace HoneyShop.Services.Core.Contracts
{
    using HoneyShop.ViewModels.Order;

    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(string userId, CreateOrderViewModel model);

        Task<OrderConfirmationViewModel?> GetOrderConfirmationAsync(Guid orderId);

        Task<IEnumerable<GetAllOrdersForUserViewModel>> GetUserOrdersAsync(string userId);
    }
}
