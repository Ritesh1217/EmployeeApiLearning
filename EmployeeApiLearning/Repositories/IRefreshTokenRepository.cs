using EmployeeApiLearning.Models;

namespace EmployeeApiLearning.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetValidTokenAsync(string token);
        void Add(RefreshToken token);
        void Update(RefreshToken token);
    }
}
