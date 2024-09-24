using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Service.src.NotificationService
{
    public class SmsManagement : ISmsManagement
    {
        public async Task SendSMSAsync(string phoneNumber, string message)
        {
            Console.WriteLine($"SMS sent to {phoneNumber}: {message}");
            await Task.CompletedTask;
        }
    }
}