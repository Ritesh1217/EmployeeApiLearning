using EmployeeApiLearning.Models;

namespace EmployeeApiLearning.Repositories
{
    public interface IAuthRepository
    {
        Task<AppUser?> GetUserByUsername(string username);
        Task<AppUser?> GetUserByEmployeeCode(string employeeCode);
        Task AddUser(AppUser user);
        Task SaveChanges();
    }
}
