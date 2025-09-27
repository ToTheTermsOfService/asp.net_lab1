using Lab_1_utilities.Interfaces;
using Lab_1_utilities.Services;

namespace Lab_1_utilities.Data
{
    public static class ServiceInjector
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITenantServiceRepository, TenantServiceRepository>();
            return services;
        }
    }
}
