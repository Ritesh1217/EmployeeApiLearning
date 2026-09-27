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
        private readonly IUnitOfWork _unitOfWork;
             
        public EmployeeService(AppDbContext context, 
            IEmployeeHelper employeeHelper,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _employeeHelper = employeeHelper;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmployeeResponseDto>> GetAllEmployees()
        {
            var employees = await _unitOfWork.Employees.GetAllAsync();

            return _mapper.Map<List<EmployeeResponseDto>>(employees);
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

            _unitOfWork.Employees.Add(employee);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EmployeeResponseDto>(employee);
        }
        public async Task<EmployeeResponseDto?> UpdateEmployee(string employeeCode, EmployeeDto employeeDto)
        {
            var employee = await _unitOfWork.Employees.GetByCodeAsync(employeeCode);

            if (employee == null)
                return null;

            _mapper.Map(employeeDto, employee);

            _unitOfWork.Employees.Update(employee);
            await _unitOfWork.SaveChangesAsync();

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
