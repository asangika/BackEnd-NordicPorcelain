using Ecommerce.Domain.src.Entities.PaymentAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Service.src.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaymentController : AppController<Payment, PaymentReadDto, PaymentCreateDto, PaymentUpdateDto>
    {
        private readonly IPaymentManagement _paymentManagement;

        public PaymentController(IPaymentManagement paymentManagement) : base(paymentManagement)
        {
            _paymentManagement = paymentManagement;
        }

        // GET: api/v1/Payment/User/{userId}
        [HttpGet("User/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetAllUserPaymentAsync(Guid userId)
        {
            var payments = await _paymentManagement.GetAllUserPaymentAsync(userId);
            if (payments == null)
            {
                return NotFound("No payments found for this user.");
            }
            return Ok(payments);
        }
    }
}
