using Microsoft.AspNetCore.SignalR;
using orchid_backend_net.Application.Common.Interfaces;

namespace orchid_backend_net.Infrastructure.Service
{
    public class HubNotificationService(IHubContext<NotificationHub> hub) : IHubnotificationService
    {
        public async Task PushToUserAsync(string userId, string title, string content)
        {
            await hub.Clients.User(userId)
                .SendAsync("notification:new", new
                {
                    id = Guid.NewGuid().ToString(),
                    userId,
                    title,
                    content,
                    isRead = false,
                    createdAt = DateTime.UtcNow
                });
        }
    }
}
