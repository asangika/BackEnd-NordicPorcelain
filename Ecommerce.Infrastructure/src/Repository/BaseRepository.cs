using Ecommerce.Domain.src.Interface;
using Ecommerce.Domain.src.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Ecommerce.Domain.src.Model;

namespace Ecommerce.Infrastructure.src.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateByIdAsync(T entity)
        {
            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PaginatedResult<T>> GetAllAsync(PaginationOptions paginationOptions)
        {
            var totalEntity = await _dbSet.CountAsync();
            IQueryable<T> query = _dbSet;
            var entities = await query
                .Skip((paginationOptions.Page - 1) * paginationOptions.PerPage)
                .Take(paginationOptions.PerPage)
                .ToListAsync();

            return new PaginatedResult<T>
            {
                Items = entities,
                TotalPages = (int)Math.Ceiling(totalEntity / (double)paginationOptions.PerPage),
                CurrentPage = paginationOptions.Page,
            };

        }
    }
}