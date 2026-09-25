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
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
