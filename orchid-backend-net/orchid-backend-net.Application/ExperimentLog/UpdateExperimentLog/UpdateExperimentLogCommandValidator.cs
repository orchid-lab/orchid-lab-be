using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UpdateExperimentLog
{
    public class UpdateExperimentLogCommandValidator : AbstractValidator<UpdateExperimentLogCommand>
    {
        private readonly ITissueCultureBatchRepository _tissueCultureBatchRepository;
        private readonly IMethodRepository _methodRepository;
        private readonly ISeedlingRepository _seedlingRepository;
        private readonly IExperimentLogRepository _experimentLogRepository;

        public UpdateExperimentLogCommandValidator(ITissueCultureBatchRepository tissueCultureBatchRepository, IMethodRepository methodRepository,
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
            RuleFor(x => x.Description.Length)
                .LessThanOrEqualTo(200)
                .WithMessage("Description is too long.");
            RuleFor(x => x.Hybridization.Count())
                .LessThanOrEqualTo(2)
                .GreaterThanOrEqualTo(1)
                .WithMessage("chosing parents error.");
            RuleFor(x => x.TissueCultureBatchID)
              .MustAsync(async (id, cancellationToken) => await IsTissueCultureBatchExist(id, cancellationToken))
              .WithMessage(x => $"Not found TissueCultureBatch with ID: {x.TissueCultureBatchID}");
            RuleFor(x => x.MethodID)
                .MustAsync(async (id, cancellationToken) => await IsMethodExist(id, cancellationToken))
                .WithMessage(x => $"Not found Method with ID: {x.MethodID}");
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

        private async Task<bool> IsValidHybridzationBasedOnType(UpdateExperimentLogCommand command, CancellationToken cancellationToken)
        {
            var experimentLog = await _experimentLogRepository.FindAsync(x => x.ID.Equals(command.ID), cancellationToken);
            if (experimentLog == null)
                return false;

            //case 0: both method and hybrid is null
            if (string.IsNullOrWhiteSpace(command.MethodID) && command.Hybridization == null)
                return true;

            //case 1.a: check method to see type
            var methodID = command.MethodID ?? experimentLog.ID;
            var method = await _methodRepository.FindAsync(x => x.ID.Equals(command.MethodID) && x.Status, cancellationToken);
            if (method == null) return false;

            //case 1.b: get hybrid
            var hybrid = command.Hybridization ?? experimentLog.Hybridizations.Select(x => x.ParentID).ToList();

            //final case 1: check if method still the old one => test hybridzation based on type Clonal or Sexual
            //if method is the new one => test hybridzation based on type Clonal or Sexual
            return method.Type switch
            {
                "Clonal" => hybrid.Count == 1,
                "Sexual" => hybrid.Count == 2,
            };
        }
    }
}
