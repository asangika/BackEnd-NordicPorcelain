using Ecommerce.Domain.src.Auth;
using Ecommerce.Domain.src.Interface;
using Ecommerce.Domain.src.UserAggregate;
using System.Linq.Expressions;

namespace Ecommerce.Domain.src.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<bool> UpdatePasswordAsync(Guid userId, string newPassword);
    }
}