using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Domain.src.Shared
{
    public class PaginatedResult<T> : BaseEntity
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Items { get; set; }
    }

}