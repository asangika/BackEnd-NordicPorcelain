using Ecommerce.Domain.Enums;
using Ecommerce.Domain.src.AddressAggregate;
using Ecommerce.Domain.src.Entities.OrderAggregate;
using Ecommerce.Domain.src.Shared;

namespace Ecommerce.Domain.src.Entities.ShipmentAggregate
{
    public class Shipment : BaseEntity
    {
        public DateTime ShipmentDate { get; set; }
        public Guid OrderId { get; set; }
        public Guid AddressId { get; set; }
        public ShippingStatus ShipmentStatus { get; set; }
        public String? TrackingNumber { get; set; }

        // Navigation properties
        public Order? Order { get; set; }
        public Address? Address { get; set; }
    }
}