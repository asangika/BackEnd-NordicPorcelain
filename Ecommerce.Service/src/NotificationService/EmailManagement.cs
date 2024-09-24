using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Service.src.NotificationService
{
    public class EmailManagement : IEmailManagement
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            Console.WriteLine($"Email sent to {to}: {subject} - {body}");
            await Task.CompletedTask;
        }

    }
}