using Ecommerce.Domain.src.Entities.ReviewAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.ReviewService;
using Moq;

namespace Ecommerce.Tests.src.Service
{
    public class ReviewServiceTests
    {
        private readonly Mock<IReviewRepository> _mockReviewRepository;
        private readonly ReviewManagement _reviewService;

        public ReviewServiceTests()
        {
            _mockReviewRepository = new Mock<IReviewRepository>();
            _reviewService = new ReviewManagement(_mockReviewRepository.Object);
        }

        [Fact]
        public async Task GetReviewsByProductIdAsync_ReviewsExist_ReturnsReviewDtos()
        {
            // Arrange
            var productId = Guid.NewGuid();

            var reviews = new List<Review>
            {
                new Review { Id = Guid.NewGuid(), ProductId = productId, Rating = 5, ReviewText = "Great product!" },
                new Review { Id = Guid.NewGuid(), ProductId = productId, Rating = 4, ReviewText = "Very good product!" }
            };

            _mockReviewRepository
                .Setup(repo => repo.GetReviewsByProductIdAsync(productId))
                .ReturnsAsync(reviews);

            // Act
            var result = await _reviewService.GetReviewsByProductIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(5, result.First().Rating);
            Assert.Equal("Great product!", result.First().ReviewText);
        }

        [Fact]
        public async Task GetReviewsByProductIdAsync_NoReviewsExist_ReturnsEmptyList()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _mockReviewRepository
                .Setup(repo => repo.GetReviewsByProductIdAsync(productId))
                .ReturnsAsync(new List<Review>());

            // Act
            var result = await _reviewService.GetReviewsByProductIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetReviewsByUserIdAsync_ReviewsExist_ReturnsReviewDtos()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var reviews = new List<Review>
            {
                new Review { Id = Guid.NewGuid(), UserId = userId, Rating = 5, ReviewText = "Great review!" },
                new Review { Id = Guid.NewGuid(), UserId = userId, Rating = 4, ReviewText = "Very good!" }
            };

            _mockReviewRepository
                .Setup(repo => repo.GetReviewsByUserIdAsync(userId))
                .ReturnsAsync(reviews);

            // Act
            var result = await _reviewService.GetReviewsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(5, result.First().Rating);
            Assert.Equal("Great review!", result.First().ReviewText);
        }

        [Fact]
        public async Task GetReviewsByUserIdAsync_NoReviewsExist_ReturnsEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockReviewRepository
                .Setup(repo => repo.GetReviewsByUserIdAsync(userId))
                .ReturnsAsync(new List<Review>());

            // Act
            var result = await _reviewService.GetReviewsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }


    }
}