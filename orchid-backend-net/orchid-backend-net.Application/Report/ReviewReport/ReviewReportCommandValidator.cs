using FluentValidation;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Report.ReviewReport
{
    public class ReviewReportCommandValidator : AbstractValidator<ReviewReportCommand>
    {
        private readonly IReportRepository _reportRepository;
        public ReviewReportCommandValidator(IReportRepository reportRepository)
        {
            this._reportRepository = reportRepository;
            Configuration();
        }

        void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("can't empty ID.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellationToken) => await IsExsitReport(id, cancellationToken))
                .WithMessage("Not found or report was deleted");
        }

        async Task<bool> IsExsitReport(string id, CancellationToken cancellationToken)
            => await this._reportRepository.AnyAsync(x => x.ID.Equals(id) && x.Status != 2, cancellationToken);
    }
}
