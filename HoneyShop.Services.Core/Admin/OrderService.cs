namespace HoneyShop.Services.Core.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.Home;
    using HoneyShop.ViewModels.Admin.OrderManagment;
    using Microsoft.EntityFrameworkCore;

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderStatusRepository orderStatusRepository;

        public OrderService(IOrderRepository orderRepository, IOrderStatusRepository orderStatusRepository)
        {
            this.orderRepository = orderRepository;
            this.orderStatusRepository = orderStatusRepository;
        }

        public async Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync()
        {
            IEnumerable<OrderViewModel> orders = await orderRepository
                .GetAllAttached()
                .Include(o => o.OrderStatus)
                .Include(o => o.User)
                .Where(o => !o.IsDeleted)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    OrderStatus = o.OrderStatus.Name,
                    OrderStatusId = o.OrderStatusId,
                    UserEmail = o.User.Email!,
                    UserName = $"{o.User.UserName}",
                    ShippingCity = o.ShippingCity,
                    ShippingAddress = o.ShippingAddress
                })
                .ToListAsync();

            return orders;
        }

        public async Task<OrderDetailsViewModel?> GetOrderDetailsAsync(Guid orderId)
        {
            Order? order = await orderRepository
                .GetAllAttached()
                .Include(o => o.OrderStatus)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted);

            if (order == null)
            {
                return null;
            }

            IEnumerable<OrderItemViewModel> orderItems = order.OrderItems
                .Where(oi => !oi.IsDeleted)
                .Select(oi => new OrderItemViewModel
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.UnitPrice * oi.Quantity
                })
                .ToList();

            return new OrderDetailsViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus.Name,
                OrderStatusId = order.OrderStatusId,
                UserEmail = order.User.Email!,
                UserName = $"{order.User.UserName}",
                UserId = order.UserId,
                ShippingCity = order.ShippingCity,
                ShippingAddress = order.ShippingAddress,
                OrderItems = orderItems
            };
        }

        public async Task<IEnumerable<OrderStatusViewModel>> GetAllOrderStatusesAsync()
        {
            IEnumerable<OrderStatusViewModel> statuses = await orderStatusRepository
                .GetAllAttached()
                .Where(s => !s.IsDeleted)
                .Select(s => new OrderStatusViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description
                })
                .ToListAsync();

            return statuses;
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, Guid statusId)
        {
            Order? order = await orderRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted);

            if (order == null)
            {
                return false;
            }

            OrderStatus? status = await orderStatusRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(s => s.Id == statusId && !s.IsDeleted);

            if (status == null)
            {
                return false;
            }

            order.OrderStatusId = statusId;
            await orderRepository.SaveChangesAsync();
            return true;
        }

        public async Task<DashboardOrderStats> GetOrderStatisticsAsync()
        {
            System.DateTime today = DateTime.UtcNow.Date;
            System.DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            System.DateTime startOfMonth = new System.DateTime(today.Year, today.Month, 1);

            IEnumerable<Order> allOrders = await orderRepository
                .GetAllAttached()
                .Include(o => o.OrderStatus)
                .Where(o => !o.IsDeleted)
                .ToListAsync();

            DashboardOrderStats? stats = new DashboardOrderStats
            {
                TotalOrders = allOrders.Count(),
                PendingOrders = allOrders.Count(o => o.OrderStatus.Name == "Pending"),
                ConfirmedOrders = allOrders.Count(o => o.OrderStatus.Name == "Confirmed"),
                SentOrders = allOrders.Count(o => o.OrderStatus.Name == "Sent"),
                FinishedOrders = allOrders.Count(o => o.OrderStatus.Name == "Finished"),
                TotalSales = allOrders.Sum(o => o.TotalAmount),
                DailySales = allOrders.Where(o => o.OrderDate.Date == today).Sum(o => o.TotalAmount),
                WeeklySales = allOrders.Where(o => o.OrderDate >= startOfWeek).Sum(o => o.TotalAmount),
                MonthlySales = allOrders.Where(o => o.OrderDate >= startOfMonth).Sum(o => o.TotalAmount)
            };

            return stats;
        }

        public async Task<IEnumerable<RecentOrderViewModel>> GetRecentOrdersAsync(int count = 5)
        {
            IEnumerable<RecentOrderViewModel> recentOrders = await orderRepository
                .GetAllAttached()
                .Include(o => o.OrderStatus)
                .Include(o => o.User)
                .Where(o => !o.IsDeleted)
                .OrderByDescending(o => o.OrderDate)
                .Take(count)
                .Select(o => new RecentOrderViewModel
                {
                    Id = o.Id,
                    CustomerName = o.User.UserName ?? o.User.Email!,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    OrderStatus = o.OrderStatus.Name
                })
                .ToListAsync();

            return recentOrders;
        }
    }
}
