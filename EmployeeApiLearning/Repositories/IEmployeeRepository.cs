using EmployeeApiLearning.Models;

namespace EmployeeApiLearning.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee?> GetByCodeAsync(string employeeCode);
        Task<int> GetCountAsync();
        void Add(Employee employee);
        void Update(Employee employee);
        Task<bool> DeleteAsync(int id);

    }
}
