using System.ComponentModel.DataAnnotations;

namespace EmployeeApiLearning.DTO
{
    public class EmployeeDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty;

        [Required]
        public decimal Salary { get; set; }
    }
}
