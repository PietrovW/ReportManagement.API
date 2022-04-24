using FluentValidation;
using ReportManagement.Application.Request.V1;

namespace ReportManagement.API.Validators
{
    public class CreateReportRequestValidator : AbstractValidator<CreateReportRequest>
    {
        public CreateReportRequestValidator()
        {
            RuleFor(p => p.Name)
               .NotEmpty().WithMessage("{Name} should be not empty. NEVER!");
        }
    }
}
