using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Report.ReviewReport
{
    public class ReviewReportCommand : IRequest<string>, ICommand
    {
        public string ID { get; set; }
        public string? ReviewReportText { get; set; }
        public ReviewReportCommand() { }
        public ReviewReportCommand(string ID, string? ReviewReportText)
        {
            this.ID = ID;
            this.ReviewReportText = ReviewReportText;
        }
    }

    internal class ReviewReportCommandHandler(
        IReportRepository reportRepository
        ) : IRequestHandler<ReviewReportCommand, string>
    {
        public async Task<string> Handle(ReviewReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var report = await reportRepository.FindAsync(x => x.ID.Equals(request.ID) && x.Status != 2, cancellationToken);
                report.ReviewReport = request.ReviewReportText;
                report.Status = 1;
                reportRepository.Update(report);
                return (await reportRepository.UnitOfWork.SaveChangesAsync(cancellationToken)) > 0 ? "Successfully" : "Failed";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
