using Discount.Domain.Entities;
using Discount.Infrastructure.Contracts;

namespace Discount.Infrastructure.Repositories
{
    public interface ICustomerRepository : IAsyncRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetCustomerByName(string customerName);
        Task<int> GetMaxId();
    }
}