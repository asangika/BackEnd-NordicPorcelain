
using Ecommerce.Domain.src.AddressAggregate;
using Ecommerce.Domain.src.Interface;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.Shared;

namespace Ecommerce.Service.src.AddressService
{
    public class AddressManagement : BaseService<Address, AddressReadDto, AddressCreateDto, AddressUpdateDto>, IAddressManagement
    {
        private readonly IAddressRepository _addressRepository;

        public AddressManagement(IAddressRepository addressRepository) : base(addressRepository)
        {
            _addressRepository = addressRepository;
        }
    }
}