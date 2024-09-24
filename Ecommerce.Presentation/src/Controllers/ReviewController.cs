using Ecommerce.Domain.src.Entities.ReviewAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.ReviewService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReviewController : AppController<Review, ReviewReadDto, ReviewCreateDto, ReviewUpdateDto>
    {
        private readonly IReviewManagement _reviewManagement;

        public ReviewController(IReviewManagement reviewManagement) : base(reviewManagement)
        {
            _reviewManagement = reviewManagement;
        }

        [Authorize]
        public override async Task<ActionResult<ReviewReadDto>> CreateAsync(ReviewCreateDto entity)
        {
            return await base.CreateAsync(entity);
        }

        [Authorize]
        public override async Task<ActionResult<ReviewReadDto>> UpdateAsync(Guid id, ReviewUpdateDto entity)
        {
            return await base.UpdateAsync(id, entity);
        }

        [Authorize]
        public override async Task<ActionResult> DeleteAsync(Guid id)
        {
            return await base.DeleteAsync(id);
        }

        // GET: api/v1/Review/Product/{productId}
        [HttpGet("Product/{productId:guid}")]
        public async Task<IActionResult> GetReviewsByProductId(Guid productId)
        {
            var reviews = await _reviewManagement.GetReviewsByProductIdAsync(productId);
            if (reviews == null)
            {
                return NotFound("No reviews found for this product.");
            }
            return Ok(reviews);
        }

        // GET: api/v1/Review/User/{userId}
        [HttpGet("User/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetReviewsByUserId(Guid userId)
        {
            var reviews = await _reviewManagement.GetReviewsByUserIdAsync(userId);
            if (reviews == null)
            {
                return NotFound("No reviews found for this user.");
            }
            return Ok(reviews);
        }

    }
}
