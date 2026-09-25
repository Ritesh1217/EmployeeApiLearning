using EmployeeApiLearning.Models;

namespace EmployeeApiLearning.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UsernameExistsAsync(string username);
        Task<AppUser?> GetByUsernameAsync(string username);
        Task<AppUser?> GetByEmailAsync(string email);
        Task<AppUser?> GetByResetTokenAsync(string token);
        void Add(AppUser user);
        void Update(AppUser user);
    }
}
