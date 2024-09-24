using Ecommerce.Domain.src.AddressAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.AddressService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AddressController : AppController<Address, AddressReadDto, AddressCreateDto, AddressUpdateDto>
    {
        private readonly IAddressManagement _addressManagement;

        public AddressController(IAddressManagement addressManagement) : base(addressManagement)
        {
            _addressManagement = addressManagement;
        }
    }
}
