using AutoMapper;
using Discount.Application.Services;
using Discount.Domain.Entities;
using Discount.Domain.Models;
using Discount.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Test.Services
{
    public class CustomerServiceTests
    {
        [Fact]
        public async Task AddCustomer_ShouldReturnMappedCustomerDTO()
        {
            // Arrange
            var mockRepository = new Mock<ICustomerRepository>();
            var mockMapper = new Mock<IMapper>();

            // Create the CustomerDTO object to be added
            var customerDTO = new CustomerDTO
            {
                Name = "John Doe",
                SegmentType = Domain.Enums.SegmentTypesEnum.Employee

            };

            // The expected Customer object after mapping
            var expectedCustomer = new Customer
            {
                Id = 42, // For example, 42 is the maximum ID in the repository
                Name = "John Doe",
                SegmentType = "Employee"
            };

            // Set up the mock mapping behavior
            mockMapper.Setup(mapper => mapper.Map<Customer>(It.IsAny<CustomerDTO>()))
                      .Returns(expectedCustomer);
            mockMapper.Setup(mapper => mapper.Map<CustomerDTO>(It.IsAny<Customer>()))
                      .Returns(customerDTO);

            // Set up the mock repository behavior
            mockRepository.Setup(repo => repo.GetMaxId())
                          .ReturnsAsync(42); // For example, 42 is the maximum ID in the repository

            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Customer>()))
                          .ReturnsAsync(expectedCustomer);

            // Create the CustomerService with the mocked dependencies
            var customerService = new CustomerService(mockRepository.Object, mockMapper.Object);

            // Act
            var result = await customerService.AddCustomer(customerDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCustomer.Name, result.Name);
            // Assert other properties if needed
        }
    }
}
