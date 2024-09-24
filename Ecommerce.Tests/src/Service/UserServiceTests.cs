using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Domain.src.UserAggregate;
using Ecommerce.Service.src.UserService;
using Moq;

namespace Ecommerce.Tests.src.Service
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly UserManagement _userManagement;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _userManagement = new UserManagement(_mockUserRepository.Object, _mockPasswordHasher.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnUserReadDtoAndCreateUser()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                Email = "test@example.com",
                Password = "password123"
            };
            var hashedPassword = "hashedPassword";
            var salt = new byte[16];
            _mockPasswordHasher.Setup(p => p.HashPassword(createDto.Password, out hashedPassword, out salt));
            _mockUserRepository.Setup(r => r.CreateAsync(It.IsAny<User>())).Returns((User user) =>
            {
                user.Id = Guid.NewGuid();
                return Task.FromResult(user);
            });

            // Act
            var result = await _userManagement.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.Email, result.Email);
            _mockPasswordHasher.Verify(p => p.HashPassword(createDto.Password, out hashedPassword, out salt), Times.Once);
            _mockUserRepository.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
        }

        // [Fact]
        // public async Task GetByCredentialsAsync_ShouldReturnUserReadDto_WhenUserExists()
        // {
        //     // Arrange
        //     var userCredentials = new UserCredentials
        //     ("test@example.com", "password123"
        //     );
        //     var user = new User
        //     {
        //         Email = userCredentials.Email,
        //         Password = "hashedPassword",
        //         Salt = new byte[16]
        //     };
        //     _mockUserRepository.Setup(r => r.GetUserByCredentialAsync(userCredentials)).ReturnsAsync(user);

        //     // Act
        //     var result = await _userManagement.GetByCredentialsAsync(userCredentials);

        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.Equal(user.Email, result.Email);

        // }

        // [Fact]
        // public async Task GetByCredentialsAsync_ShouldThrowException_WhenUserDoesNotExist()
        // {
        //     // Arrange
        //     var userCredentials = new UserCredentials
        //     ("test@example.com", "password123"
        //     );
        //     _mockUserRepository.Setup(r => r.GetUserByCredentialAsync(userCredentials)).ReturnsAsync((User)null);

        //     // Act & Assert
        //     await Assert.ThrowsAsync<Exception>(() => _userManagement.GetByCredentialsAsync(userCredentials));

        // }

        [Fact]
        public async Task UpdatePasswordAsync_ShouldReturnTrue_WhenPasswordUpdated()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newPassword = "newPassword";
            _mockUserRepository.Setup(r => r.UpdatePasswordAsync(userId, newPassword)).ReturnsAsync(true);

            // Act
            var result = await _userManagement.UpdatePasswordAsync(userId, newPassword);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdatePasswordAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newPassword = "newPassword";
            _mockUserRepository.Setup(r => r.UpdatePasswordAsync(userId, newPassword)).ReturnsAsync(false);

            // Act
            var result = await _userManagement.UpdatePasswordAsync(userId, newPassword);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnUserReadDto_WhenUserExists()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User
            {
                Email = email,
                Password = "hashedPassword",
                Salt = new byte[16]
            };
            _mockUserRepository.Setup(r => r.GetUserByEmail(email)).ReturnsAsync(user);

            // Act
            var result = await _userManagement.GetUserByEmail(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);

        }

        [Fact]
        public async Task GetUserByEmail_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "test@example.com";
            _mockUserRepository.Setup(r => r.GetUserByEmail(email)).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userManagement.GetUserByEmail(email));
        }

    }
}

