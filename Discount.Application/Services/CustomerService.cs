using AutoMapper;
using Discount.Domain.Entities;
using Discount.Domain.Models;
using Discount.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDTO> AddCustomer(CustomerDTO customerDTO)
        {
            Customer customer = _mapper.Map<Customer>(customerDTO);
            int maxId = await _customerRepository.GetMaxId();
            customer.Id = ++maxId;
            customer = await _customerRepository.AddAsync(customer);
            return _mapper.Map<CustomerDTO>(customer);
        }
    }
}
