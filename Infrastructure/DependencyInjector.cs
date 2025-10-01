using Core.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjector
    {
        public static IServiceCollection AddServices(this IServiceCollection services) 
        {
            services.AddScoped<ITenantServiceRepository, TenantServiceRepository>();
            return services;
        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("localDb4"), b => b.MigrationsAssembly("Infrastructure")));
            return services;
        }
    }
}
