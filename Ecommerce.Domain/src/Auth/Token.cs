using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Domain.src.Auth
{
    public class Token
    {
        public Guid Id { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
    }
}