using Discount.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Discount.Infrastructure.Context.Tests
{
    public class CustomerInfoContextTests
    {
        [Fact]
        public async Task SaveChangesAsync_EntityAdded_EntityPropertiesSet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CustomerInfoContext>()
                .UseInMemoryDatabase(databaseName: "Test_CustomerInfoDB")
                .Options;

            var entity = new Customer { /* Initialize customer entity properties */ };

            using (var context = new CustomerInfoContext(options))
            {
                // Act
                context.Customer.Add(entity);
                await context.SaveChangesAsync();

                // Assert
                Assert.NotNull(entity.CreatedDate);
                Assert.NotNull(entity.CreatedBy);
            }
        }

        [Fact]
        public async Task SaveChangesAsync_EntityModified_EntityPropertiesSet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CustomerInfoContext>()
                .UseInMemoryDatabase(databaseName: "Test_CustomerInfoDB")
                .Options;

            var entity = new Customer { /* Initialize customer entity properties */ };

            using (var context = new CustomerInfoContext(options))
            {
                // Act
                context.Customer.Add(entity);
                await context.SaveChangesAsync();

                // Update the entity
                entity.Name = "Updated value";
                await context.SaveChangesAsync();

                // Assert
                Assert.NotNull(entity.LastModifiedDate);
                Assert.NotNull(entity.ModifiedBy);
            }
        }

    }

}