
namespace Ecommerce.Service.src.UserService
{
    public interface IPasswordHasher
    {
        void HashPassword(string originalPassword, out string hashPassword, out byte[] salt);
        bool VerifyPassword(string inputPassword, string storedPassword, byte[] salt);
    }
}