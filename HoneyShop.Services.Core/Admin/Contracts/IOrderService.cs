using HoneyShop.ViewModels.Admin.OrderManagment;

namespace HoneyShop.Services.Core.Admin.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync();
        Task<OrderDetailsViewModel?> GetOrderDetailsAsync(Guid orderId);
        Task<IEnumerable<OrderStatusViewModel>> GetAllOrderStatusesAsync();
        Task<bool> UpdateOrderStatusAsync(Guid orderId, Guid statusId);
    }
}
