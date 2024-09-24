namespace Ecommerce.Service.src.NotificationService
{
    public interface ISmsManagement
    {
        Task SendSMSAsync(string phoneNumber, string message);

    }
}