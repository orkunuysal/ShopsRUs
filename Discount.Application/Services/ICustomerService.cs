using Discount.Domain.Models;

namespace Discount.Application.Services
{
    public interface ICustomerService
    {
        Task<CustomerDTO> AddCustomer(CustomerDTO customerDTO);
    }
}