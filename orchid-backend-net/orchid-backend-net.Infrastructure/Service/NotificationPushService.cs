using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Infrastructure.Service
{
    internal class NotificationPushService(
        IHubnotificationService hubService,
        IFirebaseMessagingService firebaseService,
        IUserRepository userRepository) : INotificationPushService
    {
        public async Task PushToMultipleUserAsync(IEnumerable<string> userIds, string title, string content)
        {
            var jobs = userIds.Select(id => PushToSingleUserAsync(id, title, content));
            await Task.WhenAll(jobs);
        }

        public async Task PushToSingleUserAsync(string userId, string title, string content)
        {
            // SignalR (in-app)
            await hubService.PushToUserAsync(userId, title, content);

            // FCM (ngoài app)
            var user = await userRepository.FindAsync(u => u.ID == userId);
            if (!string.IsNullOrEmpty(user?.FcmToken))
            {
                await firebaseService.SendToTokenAsync(user.FcmToken, title, content);
            }
        }
    }
}