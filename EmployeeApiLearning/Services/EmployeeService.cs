using AutoMapper;
using EmployeeApiLearning.Data;
using EmployeeApiLearning.DTO;
using EmployeeApiLearning.Helpers;
using EmployeeApiLearning.Models;
using EmployeeApiLearning.Services;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApiLearning.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly IEmployeeHelper _employeeHelper;
        private readonly IMapper _mapper;

        public EmployeeService(AppDbContext context, 
            IEmployeeHelper employeeHelper,
            IMapper mapper)
        {
            _context = context;
            _employeeHelper = employeeHelper;
            _mapper = mapper;
        }

        public async Task<List<EmployeeResponseDto>> GetAllEmployees()
        {
            var employees = await _context.Employees.ToListAsync();

            return _mapper.Map<List<EmployeeResponseDto>>(employees);
        }
        public async Task<EmployeeResponseDto?> GetEmployeeByCode(string employeeCode)
        {
            var employee = await _context.Employees
                        .FirstOrDefaultAsync(x => x.EmployeeCode == employeeCode);

            if (employee == null)
                return null;

           return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<EmployeeResponseDto> AddEmployee(EmployeeDto employeeDto)
        {
            int count = await _context.Employees.CountAsync();

            Employee employee = _mapper.Map<Employee>(employeeDto);

            employee.EmployeeCode = _employeeHelper.GenerateEmployeeCode(count +  1);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<EmployeeResponseDto?> UpdateEmployee(string employeeCode, EmployeeDto employeeDto)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeCode == employeeCode);

            if (employee == null)
                return null;

            _mapper.Map(employeeDto, employee);

            await _context.SaveChangesAsync();

            return _mapper.Map<EmployeeResponseDto>(employee);
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
