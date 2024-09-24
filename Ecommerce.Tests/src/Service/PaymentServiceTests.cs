using Ecommerce.Domain.src.Entities.PaymentAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.PaymentService;
using Moq;

namespace Ecommerce.Tests.src.Service
{
    public class PaymentServiceTests
    {
        private readonly Mock<IPaymentRepository> _mockPaymentRepository;
        private readonly PaymentManagement _paymentService;

        public PaymentServiceTests()
        {
            _mockPaymentRepository = new Mock<IPaymentRepository>();
            _paymentService = new PaymentManagement(_mockPaymentRepository.Object);
        }

        [Fact]
        public async Task GetAllUserPaymentAsync_UserHasPayments_ReturnsPaymentDtos()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Sample payment data
            var payments = new List<Payment>
            {
                new Payment { Id = Guid.NewGuid(), Amount = 100, PaymentDate = DateTime.UtcNow, UserId = userId },
                new Payment { Id = Guid.NewGuid(), Amount = 200, PaymentDate = DateTime.UtcNow.AddDays(-1), UserId = userId }
            };

            _mockPaymentRepository
                .Setup(repo => repo.GetAllUserPaymentAsync(userId))
                .ReturnsAsync(payments);

            // Act
            var result = await _paymentService.GetAllUserPaymentAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(100, result.First().Amount);
        }

        [Fact]
        public async Task GetAllUserPaymentAsync_UserHasNoPayments_ReturnsEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Mock repository returns no payments for the user
            _mockPaymentRepository
                .Setup(repo => repo.GetAllUserPaymentAsync(userId))
                .ReturnsAsync(new List<Payment>());

            // Act
            var result = await _paymentService.GetAllUserPaymentAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}