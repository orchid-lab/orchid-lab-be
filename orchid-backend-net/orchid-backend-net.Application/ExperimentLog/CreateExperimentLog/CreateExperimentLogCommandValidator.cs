using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.CreateExperimentLog
{
    public class CreateExperimentLogCommandValidator : AbstractValidator<CreateExperimentLogCommand>
    {
        private readonly ITissueCultureBatchRepository _tissueCultureBatchRepository;
        private readonly IMethodRepository _methodRepository;
        private readonly ISeedlingRepository _seedlingRepository;
        private readonly IExperimentLogRepository _experimentLogRepository;
        public CreateExperimentLogCommandValidator(IMethodRepository methodRepository, ITissueCultureBatchRepository tissueCultureBatchRepository,
            ISeedlingRepository seedlingRepository, IExperimentLogRepository experimentLogRepository)
        {
            _methodRepository = methodRepository;
            _tissueCultureBatchRepository = tissueCultureBatchRepository;
            _seedlingRepository = seedlingRepository;
            _experimentLogRepository = experimentLogRepository;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.TissueCultureBatchID)
                .NotEmpty()
                .NotNull()
                .WithMessage("Tissue culture batch can not null.");
            RuleFor(x => x.TissueCultureBatchID)
                .MustAsync(async (id, cancellationToken) => await IsTissueCultureBatchExist(id, cancellationToken))
                .WithMessage(x => $"Not found TissueCultureBatch with ID: {x.TissueCultureBatchID}");
            RuleFor(x => x.TissueCultureBatchID)
                .MustAsync(async (id, cancellationToken) => !await IsTissueCultureBatchFree(id, cancellationToken))
                .WithMessage(x => $"TissueCultureBatch with ID: {x.TissueCultureBatchID} is in process.");

            RuleFor(x => x.MethodID)
                .NotEmpty()
                .NotNull()
                .WithMessage("Method can not null.");
            RuleFor(x => x.MethodID)
                .MustAsync(async (id, cancellationToken) => await IsMethodExist(id, cancellationToken))
                .WithMessage(x => $"Not found Method with ID: {x.MethodID}");

            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull()
                .WithMessage("Description can not null.");
            RuleFor(x => x.Description.Length)
                .LessThanOrEqualTo(200)
                .WithMessage("Description is too long.");
            RuleFor(x => x.Hybridization)
                .NotEmpty()
                .NotNull()
                .WithMessage("Chose parent");
            RuleFor(x => x.Hybridization.Count())
                .LessThanOrEqualTo(2)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Chosing parents error: must choose Clonal or Sexual");
            //id => string of each instance in the hybridzation list
            RuleForEach(x => x.Hybridization).ChildRules(seedlings =>
            {
                seedlings.RuleFor(id => id)
                    .MustAsync(async (id, cancellationTokent) => await IsSeedlingExist(id, cancellationTokent))
                    .WithMessage(id => $"Not found parent with ID :{id}");
            });

            RuleFor(x => x)
                .MustAsync(IsValidHybridzationBasedOnType)
                .WithMessage("Hybridzation seedlings does not match method type.");
        }

        private async Task<bool> IsMethodExist(string id, CancellationToken cancellationToken)
            => await _methodRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);

        private async Task<bool> IsTissueCultureBatchExist(string id, CancellationToken cancellationToken)
            => await _tissueCultureBatchRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
        private async Task<bool> IsSeedlingExist(string id, CancellationToken cancellationToken)
            => await _seedlingRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);

        private async Task<bool> IsValidHybridzationBasedOnType(CreateExperimentLogCommand command, CancellationToken cancellationToken)
        {
            var method = await _methodRepository.FindAsync(x => x.ID.Equals(command.MethodID) && x.Status, cancellationToken);

            return method.Type switch
            {
                "Clonal" => command.Hybridization.Count == 1,
                "Sexual" => command.Hybridization.Count == 2,
            };
        }

        private async Task<bool> IsTissueCultureBatchFree(string id, CancellationToken cancellationToken)
            => await _experimentLogRepository.AnyAsync(x => x.TissueCultureBatchID.Equals(id) && x.Status == 0, cancellationToken);
    }
}
