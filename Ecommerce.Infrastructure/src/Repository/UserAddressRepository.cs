using Ecommerce.Domain.src.Entities.UserAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Infrastructure.src.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.Infrastructure.src.Repository
{
    public class UserAddressRepository : BaseRepository<UserAddress>, IUserAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public UserAddressRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UserAddress>> GetUserAddressesByUserId(Guid userId)
        {
            return await _context.UserAddress.Where(ua => ua.UserId == userId).ToListAsync();
        }

    }
}
