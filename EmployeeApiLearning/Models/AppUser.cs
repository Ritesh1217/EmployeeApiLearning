namespace EmployeeApiLearning.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string EmployeeCode { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Employee Employee { get; set; } = null!;
    }
}
