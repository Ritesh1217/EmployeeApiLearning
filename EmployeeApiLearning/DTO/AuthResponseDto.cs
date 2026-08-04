namespace EmployeeApiLearning.DTO
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
    }
}
