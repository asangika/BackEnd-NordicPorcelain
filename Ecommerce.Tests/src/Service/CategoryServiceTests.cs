using Moq;
using Ecommerce.Domain.src.CategoryAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.CategoryService;

public class CategoryManagementTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly CategoryManagement _categoryManagement;

    public CategoryManagementTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _categoryManagement = new CategoryManagement(_mockCategoryRepository.Object);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_ShouldReturnCategoryReadDtos_WhenCategoriesExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var categories = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), Name = "Pants" },
            new Category { Id = Guid.NewGuid(), Name = "Shirts" }
        };

        _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(id))
                    .ReturnsAsync(categories);

        // Act
        var result = await _categoryManagement.GetCategoryByIdAsync(id);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }
}
