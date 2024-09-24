
using System.Linq.Expressions;
using Ecommerce.Domain.src.ProductAggregate;
using Ecommerce.Domain.src.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Infrastructure.src.Database;

namespace Ecommerce.Infrastructure.src.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsByTitleAsync(string title)
        {
            var lowerCaseTitle = title.ToLower();

            return await _context.Products
                .Where(p => p.Title.ToLower().Contains(lowerCaseTitle))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetTopSellingProductsAsync(int count)
        {
            var topSellingProductIds = await _context.OrderItems
            .GroupBy(oi => oi.ProductId)
            .Select(group => new
            {
                ProductId = group.Key,
                SalesCount = group.Select(oi => oi.OrderId).Distinct().Count()
            })
            .OrderByDescending(g => g.SalesCount)
            .Take(count)
            .Select(g => g.ProductId)
            .ToListAsync();

            return await _context.Products
            .Where(p => topSellingProductIds.Contains(p.Id))
            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetInStockProductsAsync()
        {
            return await _context.Products
                .Where(p => p.Stock > 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceAscAsync()
        {
            return await _context.Products
                .Where(p => p.Stock > 0)
                .OrderBy(p => p.Price)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceDescAsync()
        {
            return await _context.Products
                .Where(p => p.Stock > 0)
                .OrderByDescending(p => p.Price)
                .ToListAsync();
        }
    }
}
