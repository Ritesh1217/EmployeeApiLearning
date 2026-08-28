using EmployeeApiLearning.Helpers;
using EmployeeApiLearning.Services;
using EmployeeApiLearning.Repositories;

namespace EmployeeApiLearning.Configurations
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeHelper, EmployeeHelper>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
