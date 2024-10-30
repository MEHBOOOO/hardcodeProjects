using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Data;

namespace OrderService.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
