using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.src.Shared;

namespace Ecommerce.Domain.src.ProductAggregate
{
    public class ProductImage : BaseEntity
    {
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? ImageURL { get; set; }

        [Required]
        public bool IsPrimary { get; set; }

        [MaxLength(200)]
        public string? ImageText { get; set; }

        // Navigation property
        public virtual Product? Product { get; set; }
    }
}