using Ecommerce.Domain.src.UserAggregate;
using Ecommerce.Domain.src.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Service.src.UserService;
using Ecommerce.Domain.src.Auth;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Domain.src.Shared;
using Ecommerce.Domain.src.Model;

namespace Ecommerce.Presentation.src.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : AppController<User, UserReadDto, UserCreateDto, UserUpdateDto>
    {
        private readonly IUserManagement _userManagement;

        public UserController(IUserManagement userManagement) : base(userManagement)
        {
            _userManagement = userManagement;
        }

        [Authorize]
        public override async Task<ActionResult<UserReadDto>> GetByIdAsync(Guid id)
        {
            return await base.GetByIdAsync(id);
        }

        [Authorize(Roles = "Admin")]
        public override async Task<ActionResult<PaginatedResult<UserReadDto>>> GetAllAsync([FromQuery] PaginationOptions paginationOptions)
        {
            return await base.GetAllAsync(paginationOptions);
        }

        public override async Task<ActionResult<UserReadDto>> CreateAsync(UserCreateDto entity)
        {
            var existingUser = await _userManagement.GetUserByEmail(entity.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "A user with this email address already exists." });
            }
            return await base.CreateAsync(entity);
        }

        [Authorize]
        public override async Task<ActionResult<UserReadDto>> UpdateAsync(Guid id, UserUpdateDto entity)
        {
            return await base.UpdateAsync(id, entity);
        }

        [Authorize]
        public override async Task<ActionResult> DeleteAsync(Guid id)
        {
            return await base.DeleteAsync(id);
        }

        // GET: api/v1/users/{userEmail}
        [Authorize(Roles = "Admin")]
        [HttpGet("email/{userEmail}")]
        public async Task<IActionResult> GetUserByEmail(string userEmail)
        {
            var user = await _userManagement.GetUserByEmail(userEmail);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        // put: api/v1/users/{userId}/{newPassword}
        [HttpPut("{userId}/{newPassword}")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(Guid userId, string newPassword)
        {
            var result = await _userManagement.UpdatePasswordAsync(userId, newPassword);
            if (!result)
            {
                return NotFound("User not found.");
            }
            return NoContent();
        }

    }
}
