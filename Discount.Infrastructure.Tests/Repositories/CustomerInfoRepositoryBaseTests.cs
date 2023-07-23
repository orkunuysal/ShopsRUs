using Discount.Domain.Common;
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

    public class CustomerInfoRepositoryBaseTests
    {
        private readonly DbContextOptions<CustomerInfoContext> _options;

        public CustomerInfoRepositoryBaseTests()
        {
            // Set up the in-memory database options
            _options = new DbContextOptionsBuilder<CustomerInfoContext>()
                .UseInMemoryDatabase(databaseName: "Test_CustomerInfoDB")
                .Options;
        }
        /*
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            using (var context = new CustomerInfoContext(_options))
            {
                context.Database.EnsureCreated();
                var repository = new CustomerInfoRepositoryBase<EntityBase>(context);
                var expectedEntities = new List<EntityBase>
                {
                    new EntityBase { Id = 1 },
                    new EntityBase { Id = 2 },
                    new EntityBase { Id = 3 }
                };
                context.AddRange(expectedEntities);
                context.SaveChanges();

                // Act
                var actualEntities = await repository.GetAllAsync();

                // Assert
                Assert.Equal(expectedEntities.Count, actualEntities.Count);
                Assert.Equal(expectedEntities.Select(e => e.Id), actualEntities.Select(e => e.Id));
            }
        }

        // Write additional test methods for other repository methods as needed
        // ...

        [Fact]
        public async Task AddAsync_ShouldAddNewEntity()
        {
            // Arrange
            using (var context = new CustomerInfoContext(_options))
            {
                context.Database.EnsureCreated();
                var repository = new CustomerInfoRepositoryBase<EntityBase>(context);
                var newEntity = new EntityBase { Id = 4 };

                // Act
                await repository.AddAsync(newEntity);
                var addedEntity = await repository.GetByIdAsync(newEntity.Id);

                // Assert
                Assert.NotNull(addedEntity);
                Assert.Equal(newEntity.Id, addedEntity.Id);
            }
        }

        // Write additional test methods for other repository methods as needed
        // ...

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            using (var context = new CustomerInfoContext(_options))
            {
                context.Database.EnsureCreated();
                var repository = new CustomerInfoRepositoryBase<EntityBase>(context);
                var existingEntity = new EntityBase { Id = 5 };
                context.Add(existingEntity);
                context.SaveChanges();

                // Act
                existingEntity.ModifiedBy = "Updated value";
                await repository.UpdateAsync(existingEntity);
                var updatedEntity = await repository.GetByIdAsync(existingEntity.Id);

                // Assert
                Assert.NotNull(updatedEntity);
                Assert.Equal(existingEntity.Id, updatedEntity.Id);
                Assert.Equal("Updated value", updatedEntity.ModifiedBy);
            }
        }

        // Write additional test methods for other repository methods as needed
        // ...

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            // Arrange
            using (var context = new CustomerInfoContext(_options))
            {
                context.Database.EnsureCreated();
                var repository = new CustomerInfoRepositoryBase<EntityBase>(context);
                var entityToDelete = new EntityBase { Id = 6 };
                context.Add(entityToDelete);
                context.SaveChanges();

                // Act
                await repository.DeleteAsync(entityToDelete);
                var deletedEntity = await repository.GetByIdAsync(entityToDelete.Id);

                // Assert
                Assert.Null(deletedEntity);
            }
        }

        // Write additional test methods for other repository methods as needed
        // ...
        */
    }
}
