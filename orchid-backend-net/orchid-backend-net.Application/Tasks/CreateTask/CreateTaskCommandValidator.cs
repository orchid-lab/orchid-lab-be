using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.CreateTask
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        private readonly IExperimentLogRepository _experimentLogRepository;
        private readonly IStageRepository _stageRepository;
        private readonly ISampleRepository _sampleRepository;
        public CreateTaskCommandValidator(IStageRepository stageRepository, IExperimentLogRepository experimentLogRepository,
            ISampleRepository sampleRepository)
        {
            _stageRepository = stageRepository;
            _experimentLogRepository = experimentLogRepository;
            _sampleRepository = sampleRepository;
            Configuration();
        }

        void Configuration()
        {
            RuleFor(x => x.End_date)
                .GreaterThan(x => x.Start_date)
                .WithMessage("Task can not end before start time.");
            RuleFor(x => x.End_date)
                .NotNull()
                .NotEmpty()
                .WithMessage("End time can not be empty.");
            RuleFor(x => x.Start_date)
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Task can not start before create time.");
            RuleFor(x => x.Start_date)
                .NotNull()
                .NotEmpty()
                .WithMessage("Start time can not be empty.");
            RuleFor(x => x.End_date)
                .NotNull()
                .NotEmpty()
                .WithMessage("End time can not be empty.");

            //RuleFor(x => x.ExperimentLogID)
            //    .NotNull()
            //    .NotEmpty()
            //    .WithMessage("Experiment log cannot be null or empty.");
            //RuleFor(x => x.ExperimentLogID)
            //    .MustAsync(async (id, cancellationToken) => await IsExperimentLogExist(id, cancellationToken))
            //    .WithMessage(x => $"Cannot find experiment log with id: {x.ExperimentLogID}.");

            //RuleFor(x => x.StageID)
            //    .NotNull()
            //    .NotEmpty()
            //    .WithMessage("Stage cannot be null or empty.");
            //RuleFor(x => x.StageID)
            //    .MustAsync(async (id, cancellationToken) => await IsStageExist(id, cancellationToken))
            //    .WithMessage(x => $"Cannot find stage with id: {x.StageID}.");

            //RuleFor(x => x.SampleID)
            //    .NotNull()
            //    .NotEmpty()
            //    .WithMessage("Sample cannot be null or empty.");
            //RuleFor(x => x.SampleID)
            //    .MustAsync(async (id, cancellationToken) => await IsSampleExist(id, cancellationToken))
            //    .WithMessage(x => $"Cannot find sample with id: {x.SampleID}");
        }

        private async Task<bool> IsStageExist(string stageId, CancellationToken cancellationToken)
            => await _stageRepository.AnyAsync(x => x.ID.Equals(stageId), cancellationToken);

        private async Task<bool> IsExperimentLogExist(string experimentLogId, CancellationToken cancellationToken)
            => await _experimentLogRepository.AnyAsync(x => x.ID.Equals(experimentLogId), cancellationToken);

        private async Task<bool> IsSampleExist(string sampleId, CancellationToken cancellationToken)
            => await _sampleRepository.AnyAsync(x => x.ID.Equals(sampleId), cancellationToken);
    }
}
