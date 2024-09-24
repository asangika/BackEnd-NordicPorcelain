using Ecommerce.Domain.src.AddressAggregate;
using Ecommerce.Domain.src.Interfaces;
using Ecommerce.Infrastructure.src.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.Infrastructure.src.Repository
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
