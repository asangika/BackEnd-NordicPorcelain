using Ecommerce.Domain.Enums;

namespace Ecommerce.Service.src.NotificationService
{
    public interface INotificationService
    {
        Task NotifyAsync(Guid userId, string message, NotificationType notificationType);

    }
}