using EmployeeApiLearning.Data;
using EmployeeApiLearning.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApiLearning.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AppUser?> GetUserByUsername(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);
        }
        public async Task<AppUser?> GetUserByEmployeeCode(string employeeCode)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.EmployeeCode == employeeCode); 
        }
        public async Task AddUser(AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
