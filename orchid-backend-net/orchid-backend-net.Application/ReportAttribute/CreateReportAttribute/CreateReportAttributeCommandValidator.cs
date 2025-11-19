using FluentValidation;

namespace orchid_backend_net.Application.ReportAttribute.CreateReportAttribute
{
    public class CreateReportAttributeCommandValidator : AbstractValidator<CreateReportAttributeCommand>
    {
        public CreateReportAttributeCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Attribute name is required.");
            RuleFor(x => x.Value).GreaterThanOrEqualTo(0).WithMessage("Attribute value must be a non-negative number.");
        }
    }
}
