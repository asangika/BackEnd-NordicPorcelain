using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Domain.Enums;
using Ecommerce.Service.src.UserService;

namespace Ecommerce.Service.src.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailManagement _emailService;
        private readonly ISmsManagement _smsService;
        private readonly IUserManagement _userService;

        public NotificationService(IEmailManagement emailService, ISmsManagement smsService, IUserManagement userService)
        {
            _emailService = emailService;
            _smsService = smsService;
            _userService = userService;
        }

        public async Task NotifyAsync(Guid userId, string message, NotificationType notificationType)
        {
            var user = await _userService.GetByIdAsync(userId);

            switch (notificationType)
            {
                case NotificationType.Email:
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        await _emailService.SendEmailAsync(user.Email, "Notification", message);
                    }
                    break;
                case NotificationType.SMS:
                    if (!string.IsNullOrEmpty(user.PhoneNumber))
                    {
                        await _smsService.SendSMSAsync(user.PhoneNumber, message);
                    }
                    break;
                case NotificationType.Both:
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        await _emailService.SendEmailAsync(user.Email, "Notification", message);
                    }
                    if (!string.IsNullOrEmpty(user.PhoneNumber))
                    {
                        await _smsService.SendSMSAsync(user.PhoneNumber, message);
                    }
                    break;
            }
        }

    }
}