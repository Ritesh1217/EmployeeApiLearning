using EmployeeApiLearning.DTO;
using FluentValidation;

namespace EmployeeApiLearning.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be atleast 3 character");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be atleast 6 character")
                .Matches(@"[A-Z]").WithMessage("Password must contain atleast one uppercase latter")
                .Matches(@"[0-9]").WithMessage("Password must contain atleast one digit");

            RuleFor(x => x.EmployeeCode)
                .NotEmpty().WithMessage("Email is required")
                .Matches(@"^EMP\d{5}$").WithMessage("Employee code must follow the format 'EMP00001'.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

        }
    }
}
