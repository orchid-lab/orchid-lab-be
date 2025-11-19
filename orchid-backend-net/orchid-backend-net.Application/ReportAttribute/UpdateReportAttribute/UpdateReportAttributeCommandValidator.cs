using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ReportAttribute.UpdateReportAttribute
{
    public class UpdateReportAttributeCommandValidator : AbstractValidator<UpdateReportAttributeCommand>
    {
        private readonly IReportAttributeRepository _reportAttributeRepository;
        public UpdateReportAttributeCommandValidator(IReportAttributeRepository reportAttributeRepository)
        {
            _reportAttributeRepository = reportAttributeRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.ReportAttributeID)
                .MustAsync(async (id, cancellationToken) => await ReportAttributeExists(id, cancellationToken));
            RuleFor(x => x.ReportAttributeID).NotEmpty().WithMessage("Report Attribute ID is required.");
            RuleFor(x => x.Name).Must((name) =>
            {
                if(string.IsNullOrEmpty(name))
                {
                    return true; // Allow null or empty names
                }
                return name.Length <= 100; // Ensure name does not exceed 100 characters
            }).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.Value).Must((value) =>
            {
                if (value == null)
                {
                    return true; // Allow null values   
                }
                return value >= 0; // Ensure non-negative values
            }).WithMessage("Value must be a non-negative number.");
        }

        private async Task<bool> ReportAttributeExists(string reportAttributeID, CancellationToken cancellationToken)
            => await _reportAttributeRepository.AnyAsync(attribute => attribute.ID.Equals(reportAttributeID), cancellationToken);
    }
}
