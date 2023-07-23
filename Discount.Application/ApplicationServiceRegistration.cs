using Discount.Application.Services;
using Discount.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Discount.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddInfrastructureServices(configuration);
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISegmentService, SegmentService>();
            services.AddScoped<IDiscountService, DiscountService>();
            return services;
        }
    }
}
