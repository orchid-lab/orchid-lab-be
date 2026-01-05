namespace orchid_backend_net.Application.Common.Interfaces
{
    public interface IHubnotificationService 
    {
        Task PushToUserAsync(string userId, string title, string content);
    }
}
