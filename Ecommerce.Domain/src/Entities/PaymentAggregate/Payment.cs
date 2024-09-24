using System.ComponentModel.DataAnnotations;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.src.Entities.OrderAggregate;
using Ecommerce.Domain.src.PaymentAggregate;
using Ecommerce.Domain.src.Shared;
using Ecommerce.Domain.src.UserAggregate;

namespace Ecommerce.Domain.src.Entities.PaymentAggregate
{
    public class Payment : BaseEntity
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(50)]
        public Guid PaymentMethodId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public PaymentStatus PaymentStatus { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Order? Order { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}