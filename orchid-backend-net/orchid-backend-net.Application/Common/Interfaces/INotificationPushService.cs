namespace orchid_backend_net.Application.Common.Interfaces
{
    public interface INotificationPushService
    {
        public Task PushToMultipleUserAsync(IEnumerable<string> userIds, string title, string content);
        public Task PushToSingleUserAsync(string userIds, string title, string content);
    }
}
