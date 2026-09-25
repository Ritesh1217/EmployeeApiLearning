using EmployeeApiLearning.Data;
using EmployeeApiLearning.DTO;
using EmployeeApiLearning.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeApiLearning.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private static readonly TimeSpan SlidingDuration = TimeSpan.FromMinutes(5);

        private MemoryCacheEntryOptions cacheOptions =>
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheDuration,
                SlidingExpiration = CacheDuration
            };

        public EmployeeRepository(AppDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public async Task<List<Employee>> GetAllAsync()
        {
            if(_memoryCache.TryGetValue("Employees", out List<Employee>? employees) && employees != null)
                return employees;
           employees = await _context.Employees.ToListAsync();
           _memoryCache.Set("Employees", employees, cacheOptions);
           return employees;
        }
        public async Task<Employee?> GetByIdAsync(int id)
        {
            var cacheKey = $"Employee_{id}";
            if (_memoryCache.TryGetValue(cacheKey, out Employee? employees) && employees != null)
                return employees;

            employees = await _context.Employees.FindAsync(id);
            if(employees != null)
                _memoryCache.Set(cacheKey, employees, cacheOptions);
            return employees;
        }
        public async Task<Employee?> GetByCodeAsync(string employeeCode)
        {
            var cacheKey = $"Employee{employeeCode}";
            if(_memoryCache.TryGetValue(cacheKey, out Employee? employees))
                return employees;

            employees = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeCode == employeeCode);
            if( employees != null)
                _memoryCache.Set(cacheKey, employees, cacheOptions);
            return employees;
        }
        public async Task<int> GetCountAsync()
        {
            if(_memoryCache.TryGetValue("EmployeeCount", out  int count))
                return count;

            count = await _context.Employees.CountAsync();
            _memoryCache.Set("EmployeeCount", count, cacheOptions);
            return count;
        }
        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            InvalidateCache(employee);
        }
        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
            InvalidateCache(employee);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            InvalidateCache(employee);
            return true;
        }
        private void InvalidateCache(Employee employee)
        {
            _memoryCache.Remove("Employees");
            _memoryCache.Remove("EmployeeCount");
            _memoryCache.Remove($"Employee_{employee.Id}");
            _memoryCache.Remove($"Employee_{employee.EmployeeCode}");
        }
    }
}
