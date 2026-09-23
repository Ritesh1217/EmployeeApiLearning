using System.ComponentModel.DataAnnotations;

namespace EmployeeApiLearning.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(
            50,
            MinimumLength = 2,
            ErrorMessage = "Username must be between 2 and 50 characters.")]
        public string Username { get; set; } = string.Empty;


        [Required(ErrorMessage = "Password is required.")]
        [StringLength(
           100,
           MinimumLength = 6,
           ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;


        [Required(ErrorMessage = "EmployeeCode is required.")]
        [RegularExpression(
            @"^EMP\d{5}$",
            ErrorMessage = "EmployeeCode must be in the format EMP00001.")]
        public string EmployeeCode { get; set; } = string.Empty;


        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        public string? Email { get; set; }
    }
}
