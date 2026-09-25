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

        public void Add(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
        }
        public void Update(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
        }
    }
}
