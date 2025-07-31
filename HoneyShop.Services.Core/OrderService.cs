namespace HoneyShop.Services.Core
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Contracts;
    using HoneyShop.ViewModels.Cart;
    using HoneyShop.ViewModels.Order;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        private readonly ICartService cartService;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IOrderStatusRepository orderStatusRepository;

        public OrderService(
            ICartService cartService,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IOrderStatusRepository orderStatusRepository)
        {
            this.cartService = cartService;
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
            this.orderStatusRepository = orderStatusRepository;
        }

        public async Task<Guid> CreateOrderAsync(string userId, CreateOrderViewModel model)
        {
            IEnumerable<GetAllCartItemsViewModel> cartItems = await cartService.GetAllCartProductsAsync(userId);

            if (!cartItems.Any())
            {
                throw new InvalidOperationException("Cannot create an order with an empty cart.");
            }

            // Get the "Pending" order status
            OrderStatus? pendingStatus = await orderStatusRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(s => s.Name == "Pending" && !s.IsDeleted);

            if (pendingStatus == null)
            {
                throw new InvalidOperationException("Required order status 'Pending' not found.");
            }

            Order order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cartItems.Sum(ci => ci.ProductDetails.Price * ci.Quantity),
                OrderStatusId = pendingStatus.Id,
                ShippingCity = model.ShippingCity,
                ShippingAddress = model.ShippingAddress,
                IsDeleted = false,
                DeletedAt = null
            };

            await orderRepository.AddAsync(order);
            await orderRepository.SaveChangesAsync();

            // Create order items from cart items
            foreach (GetAllCartItemsViewModel cartItem in cartItems)
            {
                OrderItem orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.ProductDetails.Price,
                    IsDeleted = false,
                    DeletedAt = null
                };

                await orderItemRepository.AddAsync(orderItem);
            }

            await orderItemRepository.SaveChangesAsync();

            foreach (GetAllCartItemsViewModel cartItem in cartItems)
            {
                await cartService.DeleteProductFromCartAsync(userId, new DeleteProductFromCartViewModel
                {
                    CartId = cartItem.CartId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity
                });
            }

            return order.Id;
        }

        public async Task<OrderConfirmationViewModel?> GetOrderConfirmationAsync(Guid orderId)
        {
            var order = await orderRepository
                .GetAllAttached()
                .Include(o => o.OrderStatus)
                .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted);

            if (order == null)
            {
                return null;
            }

            return new OrderConfirmationViewModel
            {
                OrderId = order.Id,
                ShippingCity = order.ShippingCity,
                ShippingAddress = order.ShippingAddress,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus.Name
            };
        }

        public async Task<IEnumerable<GetAllOrdersForUserViewModel>> GetUserOrdersAsync(string userId)
        {
            IEnumerable<Order> orders = await orderRepository
            .GetAllAttached()
            .Include(o => o.OrderStatus)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId && !o.IsDeleted)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

            List<GetAllOrdersForUserViewModel> result = new List<GetAllOrdersForUserViewModel>();

            foreach (Order order in orders)
            {
                GetAllOrdersForUserViewModel? orderViewModel = new GetAllOrdersForUserViewModel
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderStatus = order.OrderStatus.Name,
                    ShippingCity = order.ShippingCity,
                    ShippingAddress = order.ShippingAddress,
                    OrderItems = order.OrderItems
                        .Where(oi => !oi.IsDeleted)
                        .Select(oi => new OrderItemViewModel
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.Product.Name,
                            ProductPrice = oi.UnitPrice,
                            Quantity = oi.Quantity
                        })
                        .ToList()
                };

                result.Add(orderViewModel);
            }

            return result;
        }
    }
}
