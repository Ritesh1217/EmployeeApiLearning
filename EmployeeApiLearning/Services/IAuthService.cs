using EmployeeApiLearning.DTO;

namespace EmployeeApiLearning.Services
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterDto registerDto);
        Task<AuthResponseDto?> Login(LoginDto loginDto);
        Task<string> GenerateAndSaveRefreshToken(string username);
        Task<AuthResponseDto?> Refresh(string refreshToken);
        Task<bool> Revoke(string refreshToken);
    }
}
