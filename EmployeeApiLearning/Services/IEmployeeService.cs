using EmployeeApiLearning.DTO;
using EmployeeApiLearning.Models;

namespace EmployeeApiLearning.Services
{
    public interface IEmployeeService
    {
        Task<PagedResultDto<EmployeeResponseDto>> GetAllEmployees(EmployeeQueryParameters queryParameters);
        Task<EmployeeResponseDto?> GetEmployeeByCode(string employeeCode);
        Task<EmployeeResponseDto> AddEmployee(EmployeeDto employeeDto);
        Task<EmployeeResponseDto?> UpdateEmployee(string employeeCode, EmployeeDto employeeDto);
        Task<bool> DeleteEmployee(string employeeCode);
    }
}
