using EmployeeApiLearning.Data;
using EmployeeApiLearning.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeApiLearning.Services;
using EmployeeApiLearning.DTO;

namespace EmployeeApiLearning.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeResponseDto>> GetAllEmployees()
        {
            var employees = await _context.Employees.ToListAsync();

            return employees.Select(e => new EmployeeResponseDto
            {
                EmployeeCode = e.EmployeeCode,
                Name = e.Name,
                Department = e.Department,
                Salary = e.Salary,
            }).ToList();
        }
        public async Task<EmployeeResponseDto?> GetEmployeeByCode(string employeeCode)
        {
            var employee = await _context.Employees
                        .FirstOrDefaultAsync(x => x.EmployeeCode == employeeCode);

            if (employee == null)
                return null;

            return new EmployeeResponseDto
            {
                EmployeeCode = employee.EmployeeCode,
                Name = employee.Name,
                Department = employee.Department,
                Salary = employee.Salary
            };
        }
        public async Task<EmployeeResponseDto> AddEmployee(EmployeeDto employeeDto)
        {
            int count = await _context.Employees.CountAsync();

            Employee employee = new Employee
            {
                EmployeeCode = $"EMP{(count + 1):D5}",
                Name = employeeDto.Name,
                Department = employeeDto.Department,
                Salary = employeeDto.Salary
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return new EmployeeResponseDto
            {
                EmployeeCode = employee.EmployeeCode,
                Name = employee.Name,
                Department = employee.Department,
                Salary = employeeDto.Salary
            };
        }
        public async Task<EmployeeResponseDto?> UpdateEmployee(string employeeCode, EmployeeDto employeeDto)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeCode == employeeCode);

            if (employee == null)
                return null;

            employee.Name = employeeDto.Name;
            employee.Department = employeeDto.Department;
            employee.Salary = employeeDto.Salary;

            await _context.SaveChangesAsync();

            return new EmployeeResponseDto
            {
                EmployeeCode = employee.EmployeeCode,
                Name = employee.Name,
                Department = employeeDto.Department,
                Salary = employeeDto.Salary
            };
        }
        public async Task<bool> DeleteEmployee(string employeeCode)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeCode == employeeCode);

            if (employee == null) 
                return false;

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return true;

        }
    }
}
