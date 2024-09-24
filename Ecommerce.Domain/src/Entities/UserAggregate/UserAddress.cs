using Ecommerce.Domain.src.AddressAggregate;
using Ecommerce.Domain.src.Shared;
using Ecommerce.Domain.src.UserAggregate;

namespace Ecommerce.Domain.src.Entities.UserAggregate
{
    public class UserAddress : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid AddressId { get; set; }
        public bool IsDefault { get; set; } = false;

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Address? Address { get; set; }
    }
}