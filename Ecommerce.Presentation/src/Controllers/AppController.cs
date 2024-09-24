using Ecommerce.Domain.src.Model;
using Ecommerce.Domain.src.Shared;
using Ecommerce.Service.src.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ecommerce.Presentation.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AppController<T, TReadDto, TCreateDto, TUpdateDto> : ControllerBase
    where T : BaseEntity
    where TReadDto : IReadDto<T>
    where TCreateDto : ICreateDto<T>
    where TUpdateDto : IUpdateDto<T>
    {
        private readonly IBaseService<T, TReadDto, TCreateDto, TUpdateDto> _baseService;

        public AppController(IBaseService<T, TReadDto, TCreateDto, TUpdateDto> baseService)
        {
            _baseService = baseService;
        }

        [HttpGet]
        public virtual async Task<ActionResult<PaginatedResult<TReadDto>>> GetAllAsync([FromQuery] PaginationOptions paginationOptions)
        {
            try
            {
                var entities = await _baseService.GetAllAsync(paginationOptions);
                return Ok(entities);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error getting entities!.");
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TReadDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _baseService.GetByIdAsync(id);
                return Ok(entity);
            }
            catch (Exception)
            {
                return StatusCode(404, "Error getting entity!.");
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult<TReadDto>> CreateAsync(TCreateDto entity)
        {
            try
            {
                var createdEntity = await _baseService.CreateAsync(entity);
                return Ok(createdEntity);
            }
            catch (Exception)
            {
                return StatusCode(400, "Error creating entity!.");
            }
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TReadDto>> UpdateAsync(Guid id, TUpdateDto entity)
        {
            try
            {
                var updatedEntity = await _baseService.UpdateAsync(id, entity);
                return Ok(updatedEntity);
            }
            catch (Exception)
            {
                return StatusCode(404, "Error updating entity!.");
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _baseService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(404, "Error deleting entity!.");
            }
        }
    }
}