using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace orchid_backend_net.Infrastructure.Service
{
    public sealed class NotificationHub(ILogger<NotificationHub> logger) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier ?? "anonymous";
            var connectionId = Context.ConnectionId;

            if (!string.IsNullOrWhiteSpace(Context.UserIdentifier))
                await Groups.AddToGroupAsync(connectionId, userId);

            logger.LogInformation("SignalR connected: UserId={UserId}, ConnectionId={ConnectionId}", userId, connectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier ?? "anonymous";
            var connectionId = Context.ConnectionId;

            if (exception is null)
                logger.LogInformation("SignalR disconnected: UserId={UserId}, ConnectionId={ConnectionId}", userId, connectionId);
            else
                logger.LogWarning(exception, "SignalR disconnected with error: UserId={UserId}, ConnectionId={ConnectionId}", userId, connectionId);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task Ping()
        {
            await Clients.Caller.SendAsync("Pong", DateTimeOffset.UtcNow);
        }

        public Task<string> GetConnectionId()
        {
            return Task.FromResult(Context.ConnectionId);
        }
    }
}
