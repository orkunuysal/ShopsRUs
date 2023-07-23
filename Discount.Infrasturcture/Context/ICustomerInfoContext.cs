using Discount.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discount.Infrastructure.Context
{
    public interface ICustomerInfoContext
    {
        DbSet<Customer> Customer { get; set; }
        DbSet<Segment> Segment { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
