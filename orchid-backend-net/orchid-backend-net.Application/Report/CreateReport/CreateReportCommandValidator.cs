using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Report.CreateReport
{
    public class CreateReportCommandValidator : AbstractValidator<CreateReportCommand>
    {
        private readonly ISampleRepository _sampleRepository;
        private readonly IReportRepository _reportRepository;
        public CreateReportCommandValidator(ISampleRepository sampleRepository, IReportRepository reportRepository)
        {
            _sampleRepository = sampleRepository;
            _reportRepository = reportRepository;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.Description)
                .MaximumLength(300)
                .NotEmpty()
                .NotNull()
                .WithMessage("Invalid Description.");

            RuleFor(x => x.Name)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty()
                .WithMessage("Invalid name.");
            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellationToken) => !await IsDuplicatedName(name, cancellationToken))
                .WithMessage(x => $"Report with name {x.Name} has been existed.");

            RuleFor(x => x.Sample)
                .NotNull()
                .NotEmpty()
                .WithMessage("Not found Sample.");
            RuleFor(x => x.Sample)
                .MustAsync(async (id, cancellationToken) => await IsSamplleExist(id, cancellationToken))
                .WithMessage(x => "Sample not found in the system.");
        }
        private async Task<bool> IsSamplleExist(string id, CancellationToken cancellationToken)
            => await _sampleRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
        private async Task<bool> IsDuplicatedName(string name, CancellationToken cancellationToken)
            => await _reportRepository.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()), cancellationToken);
    }
}
