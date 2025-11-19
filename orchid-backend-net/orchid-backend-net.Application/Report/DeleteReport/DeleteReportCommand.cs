using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Report.DeleteReport
{
    public class DeleteReportCommand(string reportId) : IRequest<string>
    {
        public string ReportId { get; set; } = reportId;
    }

    internal class DeleteReportCommandHandler(IReportRepository reportRepository, ICurrentUserService currentUserService) : IRequestHandler<DeleteReportCommand, string>
    {
        public async Task<string> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var report = await reportRepository.FindAsync(r => r.ID.Equals(request.ReportId) && r.Delete_date == null, cancellationToken);
                report.Delete_by = currentUserService.UserId;
                report.Delete_date = DateTime.UtcNow;
                report.Status = 2;
                report.IsLatest = false;
                reportRepository.Update(report);
                return await reportRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? "Deleted report." : "Failed to delete report.";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
