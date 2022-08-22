using Basket.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BasketDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BasketDatabase")));

            services.AddScoped<IBasketDbContext>(provider => provider.GetService<BasketDbContext>());
            return services;
        }
    }
}
