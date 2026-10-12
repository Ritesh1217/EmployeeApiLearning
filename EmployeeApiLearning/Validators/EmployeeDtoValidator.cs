using EmployeeApiLearning.DTO;
using FluentValidation;

namespace EmployeeApiLearning.Validators
{
    public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required")
                .Must(d => new[] { "IT", "HR", "Finance", "Payroll" }.Contains(d))
                .WithMessage("Department must be one of: IT, HR, Finance, Payroll.");

            RuleFor(x => x.Salary)
                .GreaterThan(0).WithMessage("Salary should be greater than 0")
                .LessThanOrEqualTo(1000000).WithMessage("Salary cannot exceed 10,00,000.");
        }
    }
}
