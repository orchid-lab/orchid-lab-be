namespace orchid_backend_net.Application.Common.Interfaces
{
    public interface IFirebaseMessagingService
    {
        Task SendToTokenAsync(string token, string title, string body, CancellationToken cancellationToken = default);
        Task SendToTokensAsync(IEnumerable<string> tokens, string title, string body, CancellationToken cancellationToken = default);
    }
}