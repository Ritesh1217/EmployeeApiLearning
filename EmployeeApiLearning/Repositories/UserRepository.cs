using EmployeeApiLearning.Data;
using EmployeeApiLearning.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApiLearning.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }
        public async Task<AppUser?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username); 
        }
        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<AppUser?> GetByResetTokenAsync(string token)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.PasswordResetToken == token &&
                                          x.PasswordResetTokenExpiry > DateTime.UtcNow);
        }
        public void Add(AppUser user)
        {
            _context.Users.Add(user);
        }
        public void Update(AppUser user)
        {
            _context.Users.Update(user);
        }
    }
}
