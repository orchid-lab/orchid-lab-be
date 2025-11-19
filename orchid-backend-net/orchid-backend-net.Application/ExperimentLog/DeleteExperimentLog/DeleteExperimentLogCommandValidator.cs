using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.DeleteExperimentLog
{
    public class DeleteExperimentLogCommandValidator : AbstractValidator<DeleteExperimentLogCommand>
    {
        private readonly IExperimentLogRepository _experimentLogRepository;
        public DeleteExperimentLogCommandValidator(IExperimentLogRepository experimentLogRepository)
        {
            _experimentLogRepository = experimentLogRepository;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("ID can not be null.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellationToken) => await IsExist(id, cancellationToken))
                .WithMessage(x => $"Not found ExperimentLog with ID :{x.ID}");
        }

        private async Task<bool> IsExist(string id, CancellationToken cancellationToken)
            => await _experimentLogRepository.AnyAsync(x => x.ID == id, cancellationToken);
    }
}
