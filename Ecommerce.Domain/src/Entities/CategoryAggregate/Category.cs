using System.ComponentModel.DataAnnotations;
using Ecommerce.Domain.src.ProductAggregate;
using Ecommerce.Domain.src.Shared;

namespace Ecommerce.Domain.src.CategoryAggregate
{
    public class Category : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public Guid ParentCategoryId { get; set; }

        // Navigation Properties
        public IEnumerable<Product>? Products { get; set; }
    }
}