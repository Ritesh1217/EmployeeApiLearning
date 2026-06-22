namespace EmployeeApiLearning.DTO
{
    public class EmployeeResponseDto
    {
        public string EmployeeCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }
}
