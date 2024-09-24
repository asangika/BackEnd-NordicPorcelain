using System.Linq.Expressions;
using Ecommerce.Domain.src.CategoryAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Domain.src.ProductAggregate;
using Ecommerce.Service.src.ProductService;
using Moq;

namespace Ecommerce.Tests.src.Service
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly ProductManagement _productService;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _productService = new ProductManagement(_mockProductRepository.Object, _mockCategoryRepository.Object);
        }


        [Fact]
        public async Task GetProductsByCategoryAsync_CategoryExists_ReturnsProducts()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Test Category" };

            var products = new List<Product>
            {
            new Product { Id = Guid.NewGuid(), Title = "Product 1", CategoryId = categoryId },
            new Product { Id = Guid.NewGuid(), Title = "Product 2", CategoryId = categoryId }
            };

            _mockCategoryRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>(), true))
                .ReturnsAsync(category);

            _mockProductRepository.Setup(repo => repo.GetProductsByCategoryAsync(categoryId))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetProductsByCategoryAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Product 1", result.First().Title);
        }

        [Fact]
        public async Task GetProductsByCategoryAsync_CategoryDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _mockCategoryRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>(), true))
                .ReturnsAsync((Category)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _productService.GetProductsByCategoryAsync(categoryId));

            Assert.Equal("Error Retrieving Products!.", exception.Message);
        }

        [Fact]
        public async Task SearchProductsByTitleAsync_ProductsFound_ReturnsProductDtos()
        {
            // Arrange
            string title = "ExistingTitle";
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Title = "ExistingTitle1", Description = "Description1" },
                new Product { Id = Guid.NewGuid(), Title = "ExistingTitle2", Description = "Description2" }
            };

            _mockProductRepository.Setup(repo => repo.SearchProductsByTitleAsync(title))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.SearchProductsByTitleAsync(title);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("ExistingTitle1", result.First().Title);
        }

        [Fact]
        public async Task SearchProductsByTitleAsync_TitleIsNullOrWhitespace_ThrowsArgumentException()
        {
            // Arrange
            string title = " ";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _productService.SearchProductsByTitleAsync(title));

            Assert.Equal("Error Retrieving Products!.", exception.Message);
        }

        [Fact]
        public async Task GetProductsByPriceRangeAsync_NegativeMinPrice_ThrowsArgumentException()
        {
            // Arrange
            decimal minPrice = -1;
            decimal maxPrice = 100;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _productService.GetProductsByPriceRangeAsync(minPrice, maxPrice));

            Assert.Equal("Error Retrieving Products!.", exception.Message);
        }

        [Fact]
        public async Task GetProductsByPriceRangeAsync_NegativeMaxPrice_ThrowsArgumentException()
        {
            // Arrange
            decimal minPrice = 0;
            decimal maxPrice = -1;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _productService.GetProductsByPriceRangeAsync(minPrice, maxPrice));

            Assert.Equal("Error Retrieving Products!.", exception.Message);
        }

        [Fact]
        public async Task GetProductsByPriceRangeAsync_MinPriceGreaterThanMaxPrice_ThrowsArgumentException()
        {
            // Arrange
            decimal minPrice = 150;
            decimal maxPrice = 100;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _productService.GetProductsByPriceRangeAsync(minPrice, maxPrice));

            Assert.Equal("Error Retrieving Products!.", exception.Message);
        }

        [Fact]
        public async Task GetProductsByPriceRangeAsync_NoProductsFound_ReturnsEmptyList()
        {
            // Arrange
            decimal minPrice = 0;
            decimal maxPrice = 100;

            _mockProductRepository.Setup(repo => repo.GetProductsByPriceRangeAsync(minPrice, maxPrice))
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _productService.GetProductsByPriceRangeAsync(minPrice, maxPrice);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductsByPriceRangeAsync_ProductsFound_ReturnsProductDtos()
        {
            // Arrange
            decimal minPrice = 50;
            decimal maxPrice = 150;

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Title = "Product1", Price = 60, Description = "Description1" },
                new Product { Id = Guid.NewGuid(), Title = "Product2", Price = 100, Description = "Description2" }
            };

            _mockProductRepository.Setup(repo => repo.GetProductsByPriceRangeAsync(minPrice, maxPrice))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetProductsByPriceRangeAsync(minPrice, maxPrice);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Product1", result.First().Title);
        }

        [Fact]
        public async Task GetTopSellingProductsAsync_CountIsZeroOrNegative_ThrowsArgumentException()
        {
            // Arrange
            int invalidCount = 0;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _productService.GetTopSellingProductsAsync(invalidCount));

            Assert.Equal("Error Retrieving Products!.", exception.Message);
        }

        [Fact]
        public async Task GetTopSellingProductsAsync_NoProductsFound_ReturnsEmptyList()
        {
            // Arrange
            int count = 5;

            _mockProductRepository.Setup(repo => repo.GetTopSellingProductsAsync(count))
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _productService.GetTopSellingProductsAsync(count);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetTopSellingProductsAsync_ProductsFound_ReturnsProductDtos()
        {
            // Arrange
            int count = 5;

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Title = "Product1", Price = 60, Description = "Description1" },
                new Product { Id = Guid.NewGuid(), Title = "Product2", Price = 100, Description = "Description2" }
            };

            _mockProductRepository.Setup(repo => repo.GetTopSellingProductsAsync(count))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetTopSellingProductsAsync(count);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Product1", result.First().Title);
        }

        [Fact]
        public async Task GetInStockProductsAsync_ProductsInStock_ReturnsProducts()
        {
            // Arrange
            var productsInStock = new List<Product>
                {
                    new Product { Id = Guid.NewGuid(), Title = "Product1", Stock = 10 },
                    new Product { Id = Guid.NewGuid(), Title = "Product2", Stock = 5 }
                };

            _mockProductRepository.Setup(repo => repo.GetInStockProductsAsync())
                .ReturnsAsync(productsInStock);

            // Act
            var result = await _productService.GetInStockProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Product1", result.First().Title);
        }


        [Fact]
        public async Task GetInStockProductsAsync_NoProductsInStock_ReturnsEmptyList()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetInStockProductsAsync())
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _productService.GetInStockProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

    }
}