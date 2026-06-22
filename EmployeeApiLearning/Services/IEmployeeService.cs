using EmployeeApiLearning.DTO;
using EmployeeApiLearning.Models;

namespace EmployeeApiLearning.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponseDto>> GetAllEmployees();
        Task<EmployeeResponseDto?> GetEmployeeByCode(string employeeCode);
    }
}
