using Ecommerce.Domain.src.Auth;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.UserService;

namespace Ecommerce.Service.src.AuthService
{
    public class AuthManagement : IAuthManagement
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        //private readonly IUserRepo _userRepo;
        private readonly IPasswordHasher _passwordHasher;

        public AuthManagement(IUserRepository userRepository, ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> LoginAsync(UserCredentials userCredentials)
        {
            var foundUserByEmail = await _userRepository.GetUserByEmail(userCredentials.Email);
            if (foundUserByEmail == null)
            {
                throw new Exception("User not found");
            }
            var isVerified = _passwordHasher.VerifyPassword(userCredentials.Password, foundUserByEmail.Password, foundUserByEmail.Salt);
            if (isVerified)
            {
                var token = new Token
                {
                    Id = foundUserByEmail.Id,
                    Email = foundUserByEmail.Email,
                    Role = foundUserByEmail.Role
                };
                return _tokenService.GenerateToken(token);
            }
            else
            {
                throw new Exception("Invalid password");
            }
        }

        public async Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}