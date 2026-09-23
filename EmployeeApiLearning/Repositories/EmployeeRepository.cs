using EmployeeApiLearning.Data;
using EmployeeApiLearning.DTO;
using EmployeeApiLearning.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApiLearning.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Employee>> GetALlEmployees()
        {
           return await _context.Employees.ToListAsync();
        }
        public async Task<Employee?> GetEmployeeByCode(string employeeCode)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(x => x.EmployeeCode == employeeCode);
        }
        public async Task<int> GetEmployeeCount()
        {
            return await _context.Employees.CountAsync();
        }
        public async Task AddEmployee(Employee employee)
        {
            _context.Employees .Add(employee);
            await _context.SaveChangesAsync();
        }
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteEmployee(Employee employee)
        {
            _context.Employees .Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
