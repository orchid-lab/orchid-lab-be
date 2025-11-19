using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.GetExperimentLogInfor
{
    public class GetExperimentLogInforQueryValidator : AbstractValidator<GetExperimentLogInforQuery>
    {
        private readonly IExperimentLogRepository _experimentLogRepository;
        public GetExperimentLogInforQueryValidator(IExperimentLogRepository experimentLogRepository)
        {
            _experimentLogRepository = experimentLogRepository;
            Configuration();
        }
        public void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("ID can not empty.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellation) => await ExistExperimentLog(id, cancellation))
                .WithMessage(x => $"Not found any ExperimentLog with ID:{x.ID} in the system");
        }

        private async Task<bool> ExistExperimentLog(string id, CancellationToken cancellationToken)
        {
            return await _experimentLogRepository.AnyAsync(
                query => query.Where(x => x.ID == id),
                cancellationToken: cancellationToken
            );
        }
    }
}
