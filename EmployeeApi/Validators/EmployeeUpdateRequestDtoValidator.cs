using EmployeeApi.Models;
using FluentValidation;

namespace EmployeeApi.Validators
{
    public class EmployeeUpdateRequestDtoValidator : AbstractValidator<EmployeeUpdateRequestDto>
    {
        public EmployeeUpdateRequestDtoValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("EmployeeId must be greater than 0");

            RuleFor(x => x.EmployeeName)
                .NotEmpty().WithMessage("Employee name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            RuleFor(x => x.Designation)
                .NotEmpty().WithMessage("Designation is required")
                .MaximumLength(50).WithMessage("Designation must not exceed 50 characters");
        }
    }
}
