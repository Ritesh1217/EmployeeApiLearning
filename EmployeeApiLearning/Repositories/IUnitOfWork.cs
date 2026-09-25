namespace EmployeeApiLearning.Repositories
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        Task<int> SaveChangesAsync();
    }
}
