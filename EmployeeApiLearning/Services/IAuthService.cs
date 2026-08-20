using EmployeeApiLearning.DTO;

namespace EmployeeApiLearning.Services
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterDto registerDto);
        Task<AuthResponseDto?> Login(LoginDto loginDto);
    }
}
