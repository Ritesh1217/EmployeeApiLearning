using EmployeeApiLearning.Models;

namespace EmployeeApiLearning.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetALlEmployees();
        Task<Employee?> GetEmployeeByCode(string employeeCode);
        Task<int> GetEmployeeCount();
        Task AddEmployee(Employee employee);
        Task SaveChanges();
        Task DeleteEmployee(Employee employee);

    }
}
