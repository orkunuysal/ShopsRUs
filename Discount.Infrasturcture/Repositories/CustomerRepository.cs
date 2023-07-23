using Discount.Domain.Entities;
using Discount.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Discount.Infrastructure.Repositories
{
    public class CustomerRepository : CustomerInfoRepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(CustomerInfoContext dbContext) : base(dbContext)
        {
        }
        public async Task<IEnumerable<Customer>> GetCustomerByName(String customerName)
        {
            var customerList = await _dbContext.Customer
                                .Where(o => o.Name == customerName)
                                .ToListAsync();
            return customerList;
        }
        public async Task<int> GetMaxId()
        {
            try
            {
                return await _dbContext.Customer.MaxAsync(x => x.Id);
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

    }
}
