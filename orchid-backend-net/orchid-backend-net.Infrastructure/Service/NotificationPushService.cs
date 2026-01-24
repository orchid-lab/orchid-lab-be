using orchid_backend_net.Application.Common.Interfaces;

namespace orchid_backend_net.Infrastructure.Service
{
    internal class NotificationPushService(IHubnotificationService hubService) : INotificationPushService
    {
        public async Task PushToMultipleUserAsync(IEnumerable<string> userIds, string title, string content)
        {
            var jobs = userIds.Select(id => PushToSingleUserAsync(id, title, content));
            await Task.WhenAll(jobs);
        }

        public async Task PushToSingleUserAsync(string userIds, string title, string content)
            => await hubService.PushToUserAsync(userIds, title, content);
    }
}
