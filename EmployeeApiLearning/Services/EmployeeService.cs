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
        private readonly IUnitOfWork _unitOfWork;

        private const string EmployeeListCacheKey = "EmployeeList";
             
        public EmployeeService(AppDbContext context, 
            IEmployeeHelper employeeHelper,
            IMapper mapper,
            IEmployeeRepository employeeRepository,
            IMemoryCache cache,
            IUnitOfWork unitOfWork)
        {
            _employeeHelper = employeeHelper;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _cache = cache;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmployeeResponseDto>> GetAllEmployees()
        {
            if(_cache.TryGetValue(
                EmployeeListCacheKey,
                out List<EmployeeResponseDto>? cachedEmployees))
            {
                return cachedEmployees!;
            }
            var employees = await _unitOfWork.Employees.GetAllAsync();

            var employeeDtos = _mapper.Map<List<EmployeeResponseDto>>(employees);

            _cache.Set(EmployeeListCacheKey, employeeDtos, TimeSpan.FromDays(7));

            return employeeDtos;
        }
        public async Task<EmployeeResponseDto?> GetEmployeeByCode(string employeeCode)
        {
            var employee = await _unitOfWork.Employees.GetByCodeAsync(employeeCode);

            if (employee == null)
                return null;

           return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<EmployeeResponseDto> AddEmployee(EmployeeDto employeeDto)
        {
            int count = await _unitOfWork.Employees.GetCountAsync();

            Employee employee = _mapper.Map<Employee>(employeeDto);

            employee.EmployeeCode = _employeeHelper.GenerateEmployeeCode(count +  1);

            _employeeRepository.Add(employee);

            _cache.Remove(EmployeeListCacheKey);

            return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<EmployeeResponseDto?> UpdateEmployee(string employeeCode, EmployeeDto employeeDto)
        {
            var employee = await _unitOfWork.Employees.GetByCodeAsync(employeeCode);

            if (employee == null)
                return null;

            _mapper.Map(employeeDto, employee);

            await _unitOfWork.SaveChangesAsync();

            _cache.Remove(EmployeeListCacheKey);

            return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<bool> DeleteEmployee(string employeeCode)
        {
            var employee = await _unitOfWork.Employees.GetByCodeAsync(employeeCode);

            if (employee == null) 
                return false;

            var deleted = await _unitOfWork.Employees.DeleteAsync(employee.Id);

            if(deleted)
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            return true;

        }
    }
}
