using Ecommerce.Domain.src.ProductAggregate;
using Ecommerce.Domain.src.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Service.src.ProductImageService;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Presentation.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImageController : AppController<ProductImage, ProductImageReadDto, ProductImageCreateDto, ProductImageUpdateDto>
    {
        private readonly IProductImageManagement _productImageManagement;

        public ProductImageController(IProductImageManagement productImageManagement) : base(productImageManagement)
        {
            _productImageManagement = productImageManagement;
        }

        [Authorize(Roles = "Admin")]
        public override async Task<ActionResult<ProductImageReadDto>> CreateAsync(ProductImageCreateDto entity)
        {
            return await base.CreateAsync(entity);
        }

        [Authorize(Roles = "Admin")]
        public override async Task<ActionResult<ProductImageReadDto>> UpdateAsync(Guid id, ProductImageUpdateDto entity)
        {
            return await base.UpdateAsync(id, entity);
        }

        [Authorize(Roles = "Admin")]
        public override async Task<ActionResult> DeleteAsync(Guid id)
        {
            return await base.DeleteAsync(id);
        }

        [HttpGet("productImages/{productId}")]
        public async Task<ActionResult<IEnumerable<ProductImageReadDto>>> GetImagesByProductIdAsync(Guid productId)
        {
            try
            {
                var entities = await _productImageManagement.GetImagesByProductIdAsync(productId);
                return Ok(entities);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error getting entities!.");
            }
        }

        [HttpGet("main/{productId}")]
        public async Task<ActionResult<ProductImageReadDto>> GetMainImageForProductAsync(Guid productId)
        {
            try
            {
                var entity = await _productImageManagement.GetMainImageForProductAsync(productId);
                return Ok(entity);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error getting entity!.");
            }
        }

        [HttpGet("count/{productId}")]
        public async Task<ActionResult<int>> GetImageCountByProductIdAsync(Guid productId)
        {
            try
            {
                var count = await _productImageManagement.GetImageCountByProductIdAsync(productId);
                return Ok(count);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error getting entity!.");
            }
        }

        [HttpDelete("delete/{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> DeleteImagesByProductIdAsync(Guid productId)
        {
            try
            {
                var result = await _productImageManagement.DeleteImagesByProductIdAsync(productId);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error getting entity!.");
            }
        }
    }
}
