using System.Linq.Expressions;
using Ecommerce.Domain.src.Interface;
using Ecommerce.Domain.src.Model;
using Ecommerce.Domain.src.Shared;
using Ecommerce.Service.src.Shared;
using Moq;

namespace Ecommerce.Tests.Service
{
    public class BaseServiceTests
    {
        private readonly Mock<IBaseRepository<BaseEntity>> _mockRepo;
        private readonly BaseService<BaseEntity, MockReadDto, MockCreateDto, MockUpdateDto> _service;

        public BaseServiceTests()
        {
            _mockRepo = new Mock<IBaseRepository<BaseEntity>>();
            _service = new BaseService<BaseEntity, MockReadDto, MockCreateDto, MockUpdateDto>(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnReadDto()
        {
            // Arrange
            var createDto = new MockCreateDto();
            var entity = new BaseEntity();
            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<BaseEntity>())).Returns(Task.FromResult(entity));

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MockReadDto>(result);
            _mockRepo.Verify(r => r.CreateAsync(It.IsAny<BaseEntity>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnReadDto_WhenEntityExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new BaseEntity { Id = id };
            _mockRepo.Setup(r => r.GetAsync(It.IsAny<Expression<Func<BaseEntity, bool>>>(), true)).ReturnsAsync(entity);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MockReadDto>(result);
            _mockRepo.Verify(r => r.GetAsync(It.IsAny<Expression<Func<BaseEntity, bool>>>(), true), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowException_WhenEntityDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetAsync(It.IsAny<Expression<Func<BaseEntity, bool>>>(), true)).ReturnsAsync((BaseEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetByIdAsync(id));

            // Verify that GetAsync was called with any filter expression
            _mockRepo.Verify(r => r.GetAsync(It.IsAny<Expression<Func<BaseEntity, bool>>>(), true), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnReadDto_WhenEntityExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new BaseEntity { Id = id };
            var updateDto = new MockUpdateDto();
            var updatedEntity = new BaseEntity { Id = id }; // Entity after update

            // Mock the repository methods
            _mockRepo.Setup(r => r.GetAsync(It.IsAny<Expression<Func<BaseEntity, bool>>>(), true)).ReturnsAsync(entity);
            _mockRepo.Setup(r => r.UpdateByIdAsync(It.IsAny<BaseEntity>())).Returns(Task.FromResult(true));

            // Act
            var result = await _service.UpdateAsync(id, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MockReadDto>(result);
            _mockRepo.Verify(r => r.GetAsync(It.IsAny<Expression<Func<BaseEntity, bool>>>(), true), Times.Once);
            _mockRepo.Verify(r => r.UpdateByIdAsync(It.Is<BaseEntity>(e => e.Id == id)), Times.Once);

        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenEntityDoesNotExist()
        {
            var id = Guid.NewGuid();
            var updateDto = new MockUpdateDto();
            _mockRepo.Setup(r => r.GetAsync(It.IsAny<Expression<Func<BaseEntity, bool>>>(), true))
                    .ReturnsAsync((BaseEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(id, updateDto));
            _mockRepo.Verify(r => r.GetAsync(It.IsAny<Expression<Func<BaseEntity, bool>>>(), true), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteByIdAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.DeleteByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));

            // Act
            await _service.DeleteAsync(id);

            // Assert
            _mockRepo.Verify(r => r.DeleteByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfReadDto()
        {
            // Arrange
            var paginationOptions = new PaginationOptions();
            var entities = new PaginatedResult<BaseEntity>
            {
                Items = new List<BaseEntity> { new BaseEntity(), new BaseEntity() },
                CurrentPage = 1,
                TotalPages = 1
            };

            // Mock the repository method
            _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<PaginationOptions>())).ReturnsAsync(entities);

            // Act
            var result = await _service.GetAllAsync(paginationOptions);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PaginatedResult<MockReadDto>>(result);
            Assert.Equal(2, result.Items.Count());
            _mockRepo.Verify(r => r.GetAllAsync(It.IsAny<PaginationOptions>()), Times.Once);
        }
    }

    // Mock DTO classes for testing purposes
    public class MockReadDto : IReadDto<BaseEntity>
    {
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void FromEntity(BaseEntity entity) { }
    }

    public class MockCreateDto : ICreateDto<BaseEntity>
    {
        public BaseEntity CreateEntity() => new BaseEntity();
    }

    public class MockUpdateDto : IUpdateDto<BaseEntity>
    {
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public BaseEntity UpdateEntity(BaseEntity entity) => entity;
    }
}
