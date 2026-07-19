using AutoMapper;
using EmployeeApiLearning.Data;
using EmployeeApiLearning.DTO;
using EmployeeApiLearning.Helpers;
using EmployeeApiLearning.Models;
using EmployeeApiLearning.Repositories;
using EmployeeApiLearning.Services;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApiLearning.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeHelper _employeeHelper;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(AppDbContext context, 
            IEmployeeHelper employeeHelper,
            IMapper mapper,
            IEmployeeRepository employeeRepository)
        {
            _employeeHelper = employeeHelper;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        public async Task<List<EmployeeResponseDto>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetALlEmployees();

            return _mapper.Map<List<EmployeeResponseDto>>(employees);
        }
        public async Task<EmployeeResponseDto?> GetEmployeeByCode(string employeeCode)
        {
            var employee = await _employeeRepository.GetEmployeeByCode(employeeCode);

            if (employee == null)
                return null;

           return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<EmployeeResponseDto> AddEmployee(EmployeeDto employeeDto)
        {
            int count = await _employeeRepository.GetEmployeeCount();

            Employee employee = _mapper.Map<Employee>(employeeDto);

            employee.EmployeeCode = _employeeHelper.GenerateEmployeeCode(count +  1);

            await _employeeRepository.AddEmployee(employee);

            return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<EmployeeResponseDto?> UpdateEmployee(string employeeCode, EmployeeDto employeeDto)
        {
            var employee = await _employeeRepository.GetEmployeeByCode(employeeCode);

            if (employee == null)
                return null;

            _mapper.Map(employeeDto, employee);

            await _employeeRepository.SaveChanges();

            return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<bool> DeleteEmployee(string employeeCode)
        {
            var employee = await _employeeRepository.GetEmployeeByCode(employeeCode);

            if (employee == null) 
                return false;

            await _employeeRepository.DeleteEmployee(employee);

            return true;

        }
    }
}
