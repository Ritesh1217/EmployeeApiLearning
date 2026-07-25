using AutoMapper;
using EmployeeApiLearning.Data;
using EmployeeApiLearning.DTO;
using EmployeeApiLearning.Helpers;
using EmployeeApiLearning.Models;
using EmployeeApiLearning.Repositories;
using EmployeeApiLearning.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeApiLearning.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeHelper _employeeHelper;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMemoryCache _cache;

        private const string EmployeeListCacheKey = "EmployeeList";
             
        public EmployeeService(AppDbContext context, 
            IEmployeeHelper employeeHelper,
            IMapper mapper,
            IEmployeeRepository employeeRepository,
            IMemoryCache cache)
        {
            _employeeHelper = employeeHelper;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _cache = cache;
        }

        public async Task<List<EmployeeResponseDto>> GetAllEmployees()
        {
            if(_cache.TryGetValue(
                EmployeeListCacheKey,
                out List<EmployeeResponseDto>? cachedEmployees))
            {
                return cachedEmployees!;
            }
            var employees = await _employeeRepository.GetALlEmployees();

            var employeeDtos = _mapper.Map<List<EmployeeResponseDto>>(employees);

            _cache.Set(EmployeeListCacheKey, employeeDtos, TimeSpan.FromDays(7));

            return employeeDtos;
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

            _cache.Remove(EmployeeListCacheKey);

            return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<EmployeeResponseDto?> UpdateEmployee(string employeeCode, EmployeeDto employeeDto)
        {
            var employee = await _employeeRepository.GetEmployeeByCode(employeeCode);

            if (employee == null)
                return null;

            _mapper.Map(employeeDto, employee);

            await _employeeRepository.SaveChanges();

            _cache.Remove(EmployeeListCacheKey);

            return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<bool> DeleteEmployee(string employeeCode)
        {
            var employee = await _employeeRepository.GetEmployeeByCode(employeeCode);

            if (employee == null) 
                return false;

            await _employeeRepository.DeleteEmployee(employee);

            _cache.Remove(EmployeeListCacheKey);

            return true;

        }
    }
}
