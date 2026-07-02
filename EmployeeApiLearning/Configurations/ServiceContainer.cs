using EmployeeApiLearning.Helpers;
using EmployeeApiLearning.Services;

namespace EmployeeApiLearning.Configurations
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeHelper, EmployeeHelper>();

            return services;
        }
    }
}
