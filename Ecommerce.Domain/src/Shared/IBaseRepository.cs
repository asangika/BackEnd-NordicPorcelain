using System.Linq.Expressions;
using Ecommerce.Domain.src.Model;
using Ecommerce.Domain.src.Shared;

namespace Ecommerce.Domain.src.Interface
{
    public interface IBaseRepository<T>
        where T : BaseEntity
    {
        Task<T> CreateAsync(T entity);
        Task<bool> UpdateByIdAsync(T entity);
        Task<bool> DeleteByIdAsync(Guid id);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
        Task<PaginatedResult<T>> GetAllAsync(PaginationOptions paginationOptions);
    }
}