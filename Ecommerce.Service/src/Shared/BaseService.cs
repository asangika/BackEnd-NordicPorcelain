using Ecommerce.Domain.src.Interface;
using Ecommerce.Domain.src.Model;
using Ecommerce.Domain.src.Shared;

namespace Ecommerce.Service.src.Shared
{
    public class BaseService<T, TReadDto, TCreateDto, TUpdateDto> : IBaseService<T, TReadDto, TCreateDto, TUpdateDto>
        where T : BaseEntity
        where TReadDto : IReadDto<T>
        where TCreateDto : ICreateDto<T>
        where TUpdateDto : IUpdateDto<T>
    {
        private readonly IBaseRepository<T> _repo;

        public BaseService(IBaseRepository<T> repo)
        {
            _repo = repo;
        }
        public virtual async Task<TReadDto> CreateAsync(TCreateDto createDto)
        {
            var entity = createDto.CreateEntity();
            await _repo.CreateAsync(entity);
            var readDto = Activator.CreateInstance<TReadDto>();
            readDto.FromEntity(entity);
            return readDto;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteByIdAsync(id);
        }

        // public virtual async Task<PaginatedResult<TReadDto>> GetAllAsync(PaginationOptions paginationOptions)
        // {
        //     var entities = await _repo.GetAllAsync(paginationOptions);
        //     var convertedResult = entities.Items.Select(entity =>
        //     {
        //         var readDto = Activator.CreateInstance<TReadDto>();
        //         readDto.FromEntity(entity);
        //         return readDto;
        //     });

        //     return new PaginatedResult<TReadDto>
        //     {
        //         Items = convertedResult,
        //         CurrentPage = entities.CurrentPage,
        //         TotalPages = entities.TotalPages
        //     };
        // }

        public virtual async Task<PaginatedResult<TReadDto>> GetAllAsync(PaginationOptions paginationOptions)
        {
            // Fetch entities from the repository with pagination
            var entities = await _repo.GetAllAsync(paginationOptions);

            // Convert each entity to its corresponding ReadDto
            var convertedResult = entities.Items.Select(entity =>
            {
                var readDto = Activator.CreateInstance<TReadDto>();
                readDto.FromEntity(entity); // Ensure TReadDto implements this method
                return readDto;
            });

            // Return the paginated result with the converted DTOs
            return new PaginatedResult<TReadDto>
            {
                Items = convertedResult,
                CurrentPage = entities.CurrentPage,
                TotalPages = entities.TotalPages
            };
        }

        public virtual async Task<TReadDto> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetAsync(e => e.Id == id);
            if (entity == null)
            {
                throw new Exception("Entity not found");
            }
            var readDto = Activator.CreateInstance<TReadDto>();
            readDto.FromEntity(entity);
            return readDto;
        }

        public virtual async Task<TReadDto> UpdateAsync(Guid id, TUpdateDto updateDto)
        {
            var entity = await _repo.GetAsync(e => e.Id == id);
            if (entity == null)
            {
                throw new Exception("Entity not found");
            }
            entity = updateDto.UpdateEntity(entity);
            await _repo.UpdateByIdAsync(entity);
            var readDto = Activator.CreateInstance<TReadDto>();
            readDto.FromEntity(entity);
            return readDto;
        }
    }
}