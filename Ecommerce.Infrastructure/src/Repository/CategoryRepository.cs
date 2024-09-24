using Ecommerce.Domain.src.CategoryAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Infrastructure.src.Database;
using Ecommerce.Service.src.CategoryService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.Infrastructure.src.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoryByIdAsync(Guid userId)
        {
            return await _context.Categories.Where(c => c.Id == userId).ToListAsync();
        }
    }
}
