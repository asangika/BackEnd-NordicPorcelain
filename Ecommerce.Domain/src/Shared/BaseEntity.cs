
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain.src.Shared
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateTimestamps()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}