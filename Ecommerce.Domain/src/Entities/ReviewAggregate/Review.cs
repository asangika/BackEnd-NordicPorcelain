using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.src.ProductAggregate;
using Ecommerce.Domain.src.Shared;
using Ecommerce.Domain.src.UserAggregate;

namespace Ecommerce.Domain.src.Entities.ReviewAggregate
{
    public class Review : BaseEntity
    {
        [Required]
        public Guid ProductId { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public string? ReviewText { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }

    }
}