using EmployeeApiLearning.Models;

namespace EmployeeApiLearning.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployees();
    }
}
