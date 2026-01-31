using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Helper.CreateExperimentLogHelperInjection
{
    public sealed class CreateExperimentLogRepositories(
        IExperimentLogRepository experimentLogRepository,
        IMethodRepository methodRepository,
        ISeedlingRepository seedlingRepository,
        IUserRepository userRepository,
        IBatchesRepository batchesRepository,
        INotificationRepository notificationRepository)
    {
        public IExperimentLogRepository ExperimentLogRepository { get; } = experimentLogRepository;
        public IMethodRepository MethodRepository { get; } = methodRepository;
        public ISeedlingRepository SeedlingRepository { get; } = seedlingRepository;
        public IUserRepository UserRepository { get; } = userRepository;
        public IBatchesRepository BatchesRepository { get; } = batchesRepository;
        public INotificationRepository NotificationRepository { get; } = notificationRepository;
    }
}
