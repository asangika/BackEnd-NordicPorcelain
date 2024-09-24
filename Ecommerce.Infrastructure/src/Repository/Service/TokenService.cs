using Ecommerce.Domain.src.Auth;
using Ecommerce.Service.src.AuthService;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Ecommerce.Infrastructure.src.Repository.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(Token token)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, token.Id.ToString()),
                new Claim(ClaimTypes.Role, token.Role.ToString()),
            };
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["Jwt:Issuer"],
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = signingCredentials
            };
            var tokenObj = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(tokenObj);

        }
    }
}