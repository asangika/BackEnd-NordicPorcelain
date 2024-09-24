using Ecommerce.Domain.src.CategoryAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.Shared;

namespace Ecommerce.Service.src.CategoryService
{
    public class CategoryManagement : BaseService<Category, CategoryReadDto, CategoryCreateDto, CategoryUpdateDto>, ICategoryManagement
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryManagement(ICategoryRepository categoryRepository) : base(categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryReadDto>> GetCategoryByIdAsync(Guid userId)
        {
            var categories = await _categoryRepository.GetCategoryByIdAsync(userId);
            return categories.Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name,
            });
        }

    }
}