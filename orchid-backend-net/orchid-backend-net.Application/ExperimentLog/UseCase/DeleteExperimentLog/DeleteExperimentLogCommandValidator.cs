using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.DeleteExperimentLog
{
    internal class DeleteExperimentLogCommandValidator : AbstractValidator<DeleteExperimentLogCommand>
    {
        private readonly IExperimentLogRepository _experimentLogRepository;
        public DeleteExperimentLogCommandValidator(IExperimentLogRepository experimentLogRepository)
        {
            _experimentLogRepository = experimentLogRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Experiment log ID bắt buộc.")
                .NotNull().WithMessage("Experiment log ID không được bỏ trống.");
            RuleFor(x => x)
                .MustAsync((command, cancellationToken) => HalfOfSampleInExperimentBeingInfected(command.Id, cancellationToken))
                .WithMessage("Không thể xóa thí nghiệm khi chưa có ít nhất một nửa mẫu bị nhiễm.");
        }

        private Task<bool> HalfOfSampleInExperimentBeingInfected(string experimentLogId, CancellationToken cancellationToken)
        {
            var experimentLog = _experimentLogRepository.FindAsync(el => el.ID == experimentLogId, cancellationToken).Result;
            if (experimentLog == null)
            {
                return Task.FromResult(false);
            }
            var totalSamples = experimentLog.Samples.Count;
            var infectedSamples = experimentLog.Samples.Count(s => s.ExecutionDate is not null);
            return Task.FromResult(infectedSamples >= totalSamples / 2);
        }
    }
}
