using EmployeeApiLearning.Data;
using EmployeeApiLearning.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApiLearning.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<RefreshToken?> GetValidTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x =>
                x.Token == token &&
                !x.IsRevoked &&
                x.ExpiresAt > DateTime.UtcNow);
        }

        public async Task AddAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
        }
        public async Task UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await Task.CompletedTask;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
