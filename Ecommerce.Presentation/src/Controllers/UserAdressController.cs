using Ecommerce.Domain.src.Entities.UserAggregate;
using Ecommerce.Domain.src.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserAddressController : ControllerBase
    {
        private readonly IUserAddressRepository _userAddressRepository;

        public UserAddressController(IUserAddressRepository userAddressRepository)
        {
            _userAddressRepository = userAddressRepository;
        }

        // GET: api/v1/UserAddress/{userId}
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserAddresses(Guid userId)
        {
            var addresses = await _userAddressRepository.GetUserAddressesByUserId(userId);
            if (addresses == null || !addresses.Any())
            {
                return NotFound("User addresses not found.");
            }
            return Ok(addresses);
        }

        // POST: api/v1/UserAddress
        [HttpPost]
        public async Task<IActionResult> CreateUserAddress([FromBody] UserAddress userAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdUserAddress = await _userAddressRepository.CreateAsync(userAddress);
            return CreatedAtAction(nameof(GetUserAddresses), new { userId = createdUserAddress.UserId }, createdUserAddress);
        }

        // PUT: api/v1/UserAddress/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAddress(Guid id, [FromBody] UserAddress userAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != userAddress.Id)
            {
                return BadRequest("ID mismatch.");
            }
            var updateResult = await _userAddressRepository.UpdateByIdAsync(userAddress);
            if (!updateResult)
            {
                return NotFound("User address not found.");
            }
            return NoContent();
        }

        // DELETE: api/v1/UserAddress/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAddress(Guid id)
        {
            var deleteResult = await _userAddressRepository.DeleteByIdAsync(id);
            if (!deleteResult)
            {
                return NotFound("User address not found.");
            }
            return NoContent();
        }
    }
}
