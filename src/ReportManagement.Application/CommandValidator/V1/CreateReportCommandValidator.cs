using FluentValidation;
using ReportManagement.Application.Common.V1;

namespace ReportManagement.Application.CommandValidator.V1
{
    public class CreateReportCommandValidator: AbstractValidator<CreateReportCommand>
    {
        public CreateReportCommandValidator()
        {
            RuleFor(p => p.Name)
               .NotEmpty().WithMessage("{Name} should be not empty. NEVER!");
        }
    }
}
