
using System.ComponentModel.DataAnnotations;
using Ecommerce.Domain.src.Entities.PaymentAggregate;
using Ecommerce.Domain.src.Shared;

namespace Ecommerce.Domain.src.PaymentAggregate
{
    public class PaymentMethod : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string? PaymentType { get; set; }

        [Required]
        [StringLength(50)]
        public string? Provider { get; set; }

        [Required]
        [StringLength(50)]
        public string? CardNumber { get; set; }

        public DateTime ExpiryDate { get; set; }

        // Navigation properties
        public virtual IEnumerable<Payment>? Payments { get; set; }


    }
}