using Ecommerce.Domain.src.AddressAggregate;
using Ecommerce.Domain.src.Entities.OrderAggregate;
using Ecommerce.Domain.src.Entities.ShipmentAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.ShipmentService;
using Moq;

namespace Ecommerce.Tests.src.Service
{
    public class ShipmentServiceTests
    {
        private readonly Mock<IShipmentRepository> _mockShipmentRepository;
        private readonly ShipmentManagement _shipmentService;
        public ShipmentServiceTests()
        {
            _mockShipmentRepository = new Mock<IShipmentRepository>();
            _shipmentService = new ShipmentManagement(_mockShipmentRepository.Object);
        }

        [Fact]
        public async Task GetShipmentsByOrderIdAsync_ShipmentsExist_ReturnsShipmentDtos()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var shipments = new List<Shipment>
            {
                new Shipment
                {
                    Id = Guid.NewGuid(),
                    ShipmentDate = DateTime.UtcNow,
                    Order = new Order { Id = orderId },
                    Address = new Address { Id = Guid.NewGuid() },
                    TrackingNumber = "TRACK123"
                }
            };

            _mockShipmentRepository.Setup(repo => repo.GetShipmentsByOrderIdAsync(orderId))
                .ReturnsAsync(shipments);

            // Act
            var result = await _shipmentService.GetShipmentsByOrderIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            Assert.Equal("TRACK123", result.First().TrackingNumber);
        }

        [Fact]
        public async Task GetShipmentsByOrderIdAsync_NoShipmentsExist_ReturnsEmptyList()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mockShipmentRepository.Setup(repo => repo.GetShipmentsByOrderIdAsync(orderId))
                .ReturnsAsync(new List<Shipment>());

            // Act
            var result = await _shipmentService.GetShipmentsByOrderIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}