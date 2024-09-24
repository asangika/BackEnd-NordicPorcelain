using Ecommerce.Domain.src.CategoryAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController : AppController<Category, CategoryReadDto, CategoryCreateDto, CategoryUpdateDto>
    {
        private readonly ICategoryManagement _categoryManagement;

        public CategoryController(ICategoryManagement categoryManagement) : base(categoryManagement)
        {
            _categoryManagement = categoryManagement;
        }

        [Authorize]
        public override async Task<ActionResult<CategoryReadDto>> CreateAsync(CategoryCreateDto entity)
        {
            return await base.CreateAsync(entity);
        }
        [Authorize]
        public override async Task<ActionResult<CategoryReadDto>> UpdateAsync(Guid id, CategoryUpdateDto entity)
        {
            return await base.UpdateAsync(id, entity);
        }
        [Authorize]
        public override async Task<ActionResult> DeleteAsync(Guid id)
        {
            return await base.DeleteAsync(id);
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IEnumerable<CategoryReadDto>> GetCategoryByIdAsync(Guid userId)
        {
            return await _categoryManagement.GetCategoryByIdAsync(userId);
        }
    }
}
