using Ecommerce.Domain.src.CategoryAggregate;
using Ecommerce.Domain.src.Interface;

namespace Ecommerce.Domain.src.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoryByIdAsync(Guid userId);
    }
}