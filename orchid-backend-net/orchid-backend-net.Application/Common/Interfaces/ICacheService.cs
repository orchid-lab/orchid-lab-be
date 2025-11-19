namespace orchid_backend_net.Application.Common.Interfaces
{
    public interface ICacheService
    {
        Task SetAsync(string key, string value, TimeSpan? expiry = null);
        Task<string?> GetAsync(string key);
        Task<bool> RemoveAsync(string key);
    }
}
