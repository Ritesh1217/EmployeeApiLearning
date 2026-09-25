using EmployeeApiLearning.Data;

namespace EmployeeApiLearning.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IEmployeeRepository Employees { get; }
        public IUserRepository Users { get; }
        public IRefreshTokenRepository RefreshTokens { get; }
        public UnitOfWork(
            AppDbContext context,
            IEmployeeRepository employeeRepository,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository) 
        {
            _context = context;
            Employees = employeeRepository;
            Users = userRepository;
            RefreshTokens = refreshTokenRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
