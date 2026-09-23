using EmployeeApiLearning.Models;

namespace EmployeeApiLearning.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetValidTokenAsync(string token);
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
        Task SaveChangesAsync();
    }
}
