using Ecommerce.Domain.src.UserAggregate;
using Ecommerce.Domain.src.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Infrastructure.src.Database;
using Ecommerce.Domain.src.Auth;
using Ecommerce.Service.src.UserService;

namespace Ecommerce.Infrastructure.src.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly IPasswordHasher _passwordHasher;

        private readonly DbSet<User> _dbSet;

        public UserRepository(ApplicationDbContext context, IPasswordHasher passwordHasher) : base(context)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _dbSet = context.Set<User>();
        }



        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users
                .Include(u => u.UserAddresses)
                .ThenInclude(ua => ua.Address)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UpdatePasswordAsync(Guid userId, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }
            _passwordHasher.HashPassword(newPassword, out string hashedPassword, out byte[] salt);
            user.Password = hashedPassword;
            user.Salt = salt;
            _dbSet.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
