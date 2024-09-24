using System.ComponentModel.DataAnnotations;
using Ecommerce.Domain.src.Entities.UserAggregate;
using Ecommerce.Domain.src.Shared;

namespace Ecommerce.Domain.src.AddressAggregate
{
    public class Address : BaseEntity
    {
        [Required]
        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "City must be up to {1} characters long.")]
        public string? City { get; set; } = string.Empty;

        [Required]
        [StringLength(20, ErrorMessage = "Postal code must be up to {1} characters long.")]
        public string? PostalCode { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "Country name must be up to {1} characters long.")]
        public string? Country { get; set; } = string.Empty;

        // Navigation Properties
        public virtual IEnumerable<UserAddress>? UserAddresses { get; set; }
    }
}