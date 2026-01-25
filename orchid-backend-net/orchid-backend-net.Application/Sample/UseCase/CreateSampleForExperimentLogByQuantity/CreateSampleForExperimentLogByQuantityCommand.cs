using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.UseCase.CreateSampleByQuantity
{
    public record CreateSampleForExperimentLogByQuantityCommand(string ExperimentLogId, int Quantity) : IRequest<string>;

    internal class CreateSampleForExperimentLogByQuantityCommandHandler(
        ISampleRepository sampleRepository,
        IExperimentLogRepository experimentLogRepository,
        ICurrentUserService currentUserService) : IRequestHandler<CreateSampleForExperimentLogByQuantityCommand, string>
    {
        private const int MaxConcurrentSeedTasks = 5;
        public async Task<string> Handle(CreateSampleForExperimentLogByQuantityCommand request, CancellationToken cancellationToken)
        {
            var experiment = await experimentLogRepository.GetExperimentLogByIdAsync(request.ExperimentLogId, cancellationToken);
            var semaphore = new SemaphoreSlim(MaxConcurrentSeedTasks);

        }
    }
}
