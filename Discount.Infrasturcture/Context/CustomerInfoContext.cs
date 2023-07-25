using Discount.Domain.Common;
using Discount.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Discount.Infrastructure.Context
{
    public class CustomerInfoContext : DbContext,ICustomerInfoContext
    {
        public CustomerInfoContext()
        {
                
        }
        public CustomerInfoContext(DbContextOptions<CustomerInfoContext> options) : base(options)
        {
        }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath("C:\\Users\\Orkun\\source\\repos\\orkunuysal\\ShopsRUs\\Discount.API")
                    .AddJsonFile("appsettings.Development.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("CustomerInfoDB");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }*/
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Segment> Segment { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "Discount API";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.ModifiedBy = "Discount API";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
