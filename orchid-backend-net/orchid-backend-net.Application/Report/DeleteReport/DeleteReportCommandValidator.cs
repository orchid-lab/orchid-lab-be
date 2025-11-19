using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Report.DeleteReport
{
    public class DeleteReportCommandValidator : AbstractValidator<DeleteReportCommand>
    {
        private readonly IReportRepository _reportRepository;
        public DeleteReportCommandValidator(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.ReportId)
                .NotEmpty().WithMessage("Report ID cannot be empty.")
                .MustAsync(async (id, cancellationToken) => await ReportExist(id, cancellationToken)).WithMessage("Report does not exist or has already been deleted.");
            RuleFor(x => x.ReportId).NotEmpty().WithMessage("Report ID is required.");
        }

        private async Task<bool> ReportExist(string reportId, CancellationToken cancellationToken) 
            => await _reportRepository.AnyAsync(r => r.ID.Equals(reportId) && r.Delete_date == null, cancellationToken);    
    }
}
