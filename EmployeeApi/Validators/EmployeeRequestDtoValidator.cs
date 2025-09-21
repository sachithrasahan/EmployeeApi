using EmployeeApi.Models;
using FluentValidation;

namespace EmployeeApi.Validators
{
    public class EmployeeRequestDtoValidator : AbstractValidator<EmployeeRequestDto>
    {
        public EmployeeRequestDtoValidator()
        {
            RuleFor(x => x.EmployeeName)
                .NotEmpty().WithMessage("Employee name is required")
                .MaximumLength(100);

            RuleFor(x => x.Designation)
                .NotEmpty().WithMessage("Designation is required")
                .MaximumLength(50);
        }
    }
}
