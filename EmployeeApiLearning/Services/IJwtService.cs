namespace EmployeeApiLearning.Services
{
    public interface IJwtService
    {
        string GenerateToken(string username, string role, string employeeCode);
    }
}
