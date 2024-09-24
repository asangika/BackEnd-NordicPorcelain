using Ecommerce.Domain.src.Auth;

namespace Ecommerce.Service.src.AuthService
{
    public interface IAuthManagement
    {
        Task<string> LoginAsync(UserCredentials userCredentials);
        Task LogoutAsync();

    }
}