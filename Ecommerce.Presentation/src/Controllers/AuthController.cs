
using Ecommerce.Domain.src.Auth;
using Ecommerce.Service.src.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.src.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthManagement _authManagement;

        public AuthController(IAuthManagement authManagement)
        {
            _authManagement = authManagement;
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] UserCredentials userCredentials)
        {
            try
            {
                var token = await _authManagement.LoginAsync(userCredentials);
                return Ok(token);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error logging in!.");
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            try
            {
                await _authManagement.LogoutAsync();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error logging out!.");
            }
        }

    }
}