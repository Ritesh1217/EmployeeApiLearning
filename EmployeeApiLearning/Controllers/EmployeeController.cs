using EmployeeApiLearning.Constants;
using EmployeeApiLearning.Data;
using EmployeeApiLearning.DTO;
using EmployeeApiLearning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApiLearning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            // 1. Admin gets all employees
            if (string.Equals(role, EmployeeApiLearning.Constants.Roles.Admin, StringComparison.OrdinalIgnoreCase))
            {
                var allEmployees = await _employeeService.GetAllEmployees();
                return Ok(allEmployees);
            }

            // 2. Regular User gets only their own record
            var employeeCode = User.FindFirst("EmployeeCode")?.Value;
            if (string.IsNullOrEmpty(employeeCode))
            {
                return Forbid();
            }

            var employee = await _employeeService.GetEmployeeByCode(employeeCode);
            if (employee == null)
            {
                return NotFound(new { message = "Employee record not found." });
            }

            // Wrapped in a list so the response format stays consistent (array of objects)
            return Ok(new List<EmployeeResponseDto> { employee });
        }

        [HttpGet("{employeeCode}")]
        public async Task<IActionResult> GetEmployeeByCode(string employeeCode)
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (string.Equals(role, EmployeeApiLearning.Constants.Roles.User, StringComparison.OrdinalIgnoreCase))
            {
                var userCode = User.FindFirst("EmployeeCode")?.Value;
                if(!string.Equals(userCode, employeeCode, StringComparison.OrdinalIgnoreCase))
                {
                    return Forbid();
                }
            }
            var employee = await _employeeService.GetEmployeeByCode(employeeCode);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AddEmployee(EmployeeDto employeeDto)
        {
            var employee = await _employeeService.AddEmployee(employeeDto);

            return Ok(employee);
        }
        [HttpPut("{employeeCode}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateEmployee(string employeeCode, EmployeeDto employeeDto)
        {
            var employee = await _employeeService.UpdateEmployee(employeeCode, employeeDto);

            if(employee == null)
                return NotFound();

            return Ok(employee);
        }
        [HttpDelete("{employeeCode}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteEmployee(string employeeCode)
        {
            var deleted = await _employeeService.DeleteEmployee(employeeCode);

            if(!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
