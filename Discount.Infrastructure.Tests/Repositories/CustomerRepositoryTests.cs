using Discount.Domain.Entities;
using Discount.Infrastructure.Context;
using Discount.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Tests.Repositories
{
    public class CustomerRepositoryTests
    {
        private readonly DbContextOptions<CustomerInfoContext> _options;

        public CustomerRepositoryTests()
        {
            // Set up the in-memory database options
            _options = new DbContextOptionsBuilder<CustomerInfoContext>()
                .UseInMemoryDatabase(databaseName: "Test_CustomerInfoDB")
                .Options;
        }

        [Fact]
        public async Task GetCustomerByName_ShouldReturnMatchingCustomers()
        {
            // Arrange
            using (var context = new CustomerInfoContext(_options))
            {
                context.Database.EnsureCreated();
                var repository = new CustomerRepository(context);
                var customerName = "John Doe";
                var expectedCustomers = new List<Customer>
                {
                    new Customer { Id = 1, Name = "John Doe" },
                    new Customer { Id = 3, Name = "John Doe" }
                };
                context.AddRange(expectedCustomers);
                context.SaveChanges();

                // Act
                var actualCustomers = await repository.GetCustomerByName(customerName);

                // Assert
                Assert.Equal(2, actualCustomers.Count());
                Assert.All(actualCustomers, customer => Assert.Equal(customerName, customer.Name));
            }
        }

        [Fact]
        public async Task GetMaxId_ShouldReturnMaxCustomerId()
        {
            // Arrange
            using (var context = new CustomerInfoContext(_options))
            {
                context.Database.EnsureCreated();
                var repository = new CustomerRepository(context);
                var customers = new List<Customer>
                {
                    new Customer { Id = 1 },
                    new Customer { Id = 3 },
                    new Customer { Id = 2 },
                };
                context.AddRange(customers);
                context.SaveChanges();

                // Act
                var maxId = await repository.GetMaxId();

                // Assert
                Assert.Equal(3, maxId);
            }
        }

        // Write additional test methods for other repository methods as needed
        // ...
    }
}
