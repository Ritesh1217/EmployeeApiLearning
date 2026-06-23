using EmployeeApiLearning.DTO;
using EmployeeApiLearning.Models;

namespace EmployeeApiLearning.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponseDto>> GetAllEmployees();
        Task<EmployeeResponseDto?> GetEmployeeByCode(string employeeCode);
        Task<EmployeeResponseDto> AddEmployee(EmployeeDto employeeDto);
        Task<EmployeeResponseDto?> UpdateEmployee(string employeeCode, EmployeeDto employeeDto);
        Task<bool> DeleteEmployee(string employeeCode);
    }
}
