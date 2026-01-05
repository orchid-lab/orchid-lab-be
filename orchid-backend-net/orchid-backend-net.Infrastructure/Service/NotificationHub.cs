using Microsoft.AspNetCore.SignalR;

namespace orchid_backend_net.Infrastructure.Service
{
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if(!string.IsNullOrWhiteSpace(userId))
                Groups.AddToGroupAsync(Context.ConnectionId, userId);
            return base.OnConnectedAsync();
        }
    }
}
