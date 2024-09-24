using Ecommerce.Domain.Enums;
using Ecommerce.Domain.src.Entities.OrderAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.NotificationService;
using Ecommerce.Service.src.OrderService.Handlers;
using Ecommerce.Service.src.Shared;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Service.src.OrderService
{
    public class OrderManagement : BaseService<Order, OrderReadDto, OrderCreateDto, OrderUpdateDto>, IOrderManagement
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly INotificationService _notificationService;
        private readonly IOrderStatushandler _orderStatusHandler;
        private readonly ILogger<OrderManagement> _logger;

        public OrderManagement(IOrderRepository orderRepository, IUserRepository userRepository, INotificationService notificationService, IOrderStatushandler orderStatusHandler, IOrderItemRepository orderItemRepository, ILogger<OrderManagement> logger) : base(orderRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _orderStatusHandler = orderStatusHandler;
            _orderItemRepository = orderItemRepository;
            _logger = logger;
        }

        public async Task<OrderReadDto> CreateAsync(OrderCreateDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating order with DTO: {@OrderCreateDto}", createDto);

                var order = createDto.CreateEntity();

                await _orderRepository.CreateAsync(order);
                await _orderRepository.SaveChangesAsync();

                foreach (var item in createDto.OrderItems)
                {
                    var orderItem = item.CreateEntity();
                    await _orderItemRepository.CreateAsync(orderItem);
                }

                await _orderItemRepository.SaveChangesAsync();

                await _notificationService.NotifyAsync(order.UserId, "Your order has been placed.", NotificationType.Email);
                _logger.LogInformation("Order created successfully with ID: {OrderId}", order.Id);

                return new OrderReadDto();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating the order.Order DTO: {@OrderCreateDto}", createDto);
                throw new ApplicationException("An error occurred while creating the order.", ex);
            }

        }
        public async Task<IEnumerable<OrderReadDto>> GetOrdersByUserIdAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetAsync(u => u.Id == userId);
                if (user == null)
                    throw new ArgumentException("Invalid user.");

                var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
                var orderDtos = orders.Select(order =>
                {
                    var dto = new OrderReadDto();
                    dto.FromEntity(order);
                    return dto;
                });

                return orderDtos;
            }
            catch
            {
                throw new Exception("Error Retrieving Orders!.");
            }
        }

        public async Task<IEnumerable<OrderReadDto>> GetOrdersByStatusAsync(OrderStatus status)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByStatusAsync(status);
                var orderDtos = orders.Select(order =>
                {
                    var dto = new OrderReadDto();
                    dto.FromEntity(order);
                    return dto;
                });
                return orderDtos;
            }
            catch
            {
                throw new Exception("Error Retrieving Orders!.");
            }
        }

        public async Task<IEnumerable<OrderReadDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate);
                var orderDtos = orders.Select(order =>
                {
                    var dto = new OrderReadDto();
                    dto.FromEntity(order);
                    return dto;
                });
                return orderDtos;

            }
            catch
            {
                throw new Exception("Error Retrieving Orders!.");

            }
        }

        public async Task<decimal> GetTotalPriceByOrderIdAsync(Guid orderId)
        {
            try
            {
                var order = await _orderRepository.GetAsync(o => o.Id == orderId);
                if (order == null)
                    throw new ArgumentException("Order not found.");

                return await _orderRepository.GetTotalPriceByOrderIdAsync(orderId);
            }
            catch
            {
                throw new Exception("Error Retrieving Total price!.");

            }
        }

        public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            try
            {
                var order = await _orderRepository.GetAsync(o => o.Id == orderId);

                if (order == null)
                {
                    throw new KeyNotFoundException("Order not found");
                }

                await _orderStatusHandler.HandleOrderStatusAsync(order, status);

                order.UpdateOrderStatus(status);

                await _orderRepository.UpdateByIdAsync(order);
            }
            catch
            {
                throw new Exception("Error Updating Order Status!.");
            }
        }
    }
}