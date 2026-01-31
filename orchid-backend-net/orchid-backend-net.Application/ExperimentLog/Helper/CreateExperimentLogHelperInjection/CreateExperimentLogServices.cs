using orchid_backend_net.Application.Common.Interfaces;

namespace orchid_backend_net.Application.ExperimentLog.Helper.CreateExperimentLogHelperInjection
{
    public sealed class CreateExperimentLogServices(
        INotificationPushService pushService,
        ICurrentUserService currentUserService)
    {
        public INotificationPushService PushService { get; } = pushService;
        public ICurrentUserService CurrentUserService { get; } = currentUserService;
    }
}
