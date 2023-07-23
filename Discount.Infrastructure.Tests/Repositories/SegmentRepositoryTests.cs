using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Domain.Entities;
using Discount.Infrastructure.Context;
using Discount.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Discount.Infrastructure.Tests.Repositories
{
    public class SegmentRepositoryTests
    {
        private readonly DbContextOptions<CustomerInfoContext> _options;

        public SegmentRepositoryTests()
        {
            // Set up the in-memory database options
            _options = new DbContextOptionsBuilder<CustomerInfoContext>()
                .UseInMemoryDatabase(databaseName: "Test_CustomerInfoDB")
                .Options;
        }

        [Fact]
        public async Task GetSegmentByType_ShouldReturnMatchingSegment()
        {
            // Arrange
            using (var context = new CustomerInfoContext(_options))
            {
                context.Database.EnsureCreated();
                var repository = new SegmentRepository(context);
                var segmentType = "Employee";
                var expectedSegment = new Segment {Type = segmentType , DiscountRate = 10};
                context.AddRange(expectedSegment);
                context.SaveChanges();

                // Act
                var actualSegment = await repository.GetSegmentByType(segmentType);

                // Assert
                Assert.NotNull(actualSegment);
                Assert.Equal(expectedSegment.Type, actualSegment.Type);
                Assert.Equal(segmentType, actualSegment.Type);
            }
        }

        [Fact]
        public async Task GetSegmentByDiscountRate_ShouldReturnMatchingSegments()
        {
            // Arrange
            using (var context = new CustomerInfoContext(_options))
            {
                context.Database.EnsureCreated();
                var repository = new SegmentRepository(context);
                var discountRate = 10.5m;
                var expectedSegments = new List<Segment>
                {
                    new Segment { Type = "Loyalty", DiscountRate = discountRate },
                    new Segment { Type = "Affiliate" , DiscountRate = discountRate },
                };
                context.AddRange(expectedSegments);
                context.SaveChanges();

                // Act
                var actualSegments = await repository.GetSegmentByDiscountRate(discountRate);

                // Assert
                Assert.Equal(2, actualSegments.Count());
                Assert.All(actualSegments, segment => Assert.Equal(discountRate, segment.DiscountRate));
            }
        }

        [Fact]
        public async Task AddSegment_ShouldAddNewSegment()
        {
            // Arrange
            using (var context = new CustomerInfoContext(_options))
            {
                context.Database.EnsureCreated();
                var repository = new SegmentRepository(context);
                var newSegment = new Segment { Type = "Default" };

                // Act
                await repository.AddSegment(newSegment);
                var addedSegment = await repository.GetSegmentByType(newSegment.Type);

                // Assert
                Assert.NotNull(addedSegment);
                Assert.Equal(newSegment.Type, addedSegment.Type);
            }
        }

        // Write additional test methods for other repository methods as needed
        // ...
    }
}
