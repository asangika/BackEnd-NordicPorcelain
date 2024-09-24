using Ecommerce.Domain.Enums;
using Ecommerce.Domain.src.Entities.OrderAggregate;
using Ecommerce.Service.src.NotificationService;

namespace Ecommerce.Service.src.OrderService.Handlers
{
    public class OrderStatusHandler : IOrderStatushandler
    {
        private readonly INotificationService _notificationService;

        public OrderStatusHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task HandleOrderStatusAsync(Order order, OrderStatus newStatus)
        {
            switch (newStatus)
            {
                case OrderStatus.Pending:
                    await HandlePendingStatus(order);
                    break;

                case OrderStatus.Shipped:
                    await HandleShippedStatus(order);
                    break;

                case OrderStatus.Delivered:
                    await HandleDeliveredStatus(order);
                    break;

                case OrderStatus.Completed:
                    await HandleCompletedStatus(order);
                    break;

                case OrderStatus.Canceled:
                    await HandleCanceledStatus(order);
                    break;

                case OrderStatus.Refunded:
                    await HandleRefundedStatus(order);
                    break;

                case OrderStatus.Returned:
                    await HandleReturnedStatus(order);
                    break;

                case OrderStatus.Failed:
                    await HandleFailedStatus(order);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(newStatus), "Unknown order status");
            }
        }

        private async Task HandlePendingStatus(Order order)
        {
            await _notificationService.NotifyAsync(order.UserId, "Your order is pending.", NotificationType.Email);
        }

        private async Task HandleShippedStatus(Order order)
        {
            await _notificationService.NotifyAsync(order.UserId, "Your order has been shipped.", NotificationType.Both);
        }

        private async Task HandleDeliveredStatus(Order order)
        {
            await _notificationService.NotifyAsync(order.UserId, "Your order has been delivered.", NotificationType.Email);
        }

        private async Task HandleCompletedStatus(Order order)
        {
            await _notificationService.NotifyAsync(order.UserId, "Your order is complete.", NotificationType.Email);
        }

        private async Task HandleCanceledStatus(Order order)
        {
            await _notificationService.NotifyAsync(order.UserId, "Your order has been canceled.", NotificationType.Email);
        }

        private async Task HandleRefundedStatus(Order order)
        {
            await _notificationService.NotifyAsync(order.UserId, "Your order has been refunded.", NotificationType.Email);
        }

        private async Task HandleReturnedStatus(Order order)
        {
            await _notificationService.NotifyAsync(order.UserId, "Your order has been returned.", NotificationType.Email);
        }

        private async Task HandleFailedStatus(Order order)
        {
            await _notificationService.NotifyAsync(order.UserId, "Your order has failed.", NotificationType.Email);
        }
    }

}