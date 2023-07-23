using Discount.Infrastructure.Context;
using Discount.Infrastructure.Contracts;
using Discount.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.Infrastructure
{
    public static class InfastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CustomerInfoContext>(options =>
                     options.UseNpgsql(configuration.GetConnectionString("CustomerInfoDB")));
            services.AddScoped<ICustomerInfoContext>(provider => provider.GetService<CustomerInfoContext>());
            services.AddScoped(typeof(IAsyncRepository<>), typeof(CustomerInfoRepositoryBase<>));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISegmentRepository, SegmentRepository>();

            return services;
        }
    }
}
