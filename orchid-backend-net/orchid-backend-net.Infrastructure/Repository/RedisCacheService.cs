using Microsoft.Extensions.Caching.Distributed;
using orchid_backend_net.Application.Common.Interfaces;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class RedisCacheService(IDistributedCache cache) : ICacheService
    {
        public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            await cache.SetStringAsync(key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(5)
            });
        }

        public async Task<string?> GetAsync(string key)
        {
            var value = await cache.GetStringAsync(key);
            return value;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            var existingValue = await cache.GetStringAsync(key);
            if (existingValue != null)
            {
                await cache.RemoveAsync(key);
                return true;
            }
            return false;
        }
    }
}
