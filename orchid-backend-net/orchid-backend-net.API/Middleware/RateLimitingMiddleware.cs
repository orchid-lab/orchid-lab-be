using Microsoft.Extensions.Caching.Distributed;

namespace orchid_backend_net.API.Middleware
{
    public class RateLimitingMiddleware(RequestDelegate next, IDistributedCache cache, ILogger<RateLimitingMiddleware> logger)
    {
        private const int LIMIT = 100000; // Max requests per window
        private const int WINDOW_SECONDS = 60; // Time window in seconds
        public async Task InvokeAsync(HttpContext context)
        {
            var key = $"rl:{context.Connection.RemoteIpAddress}";
            var countString = await cache.GetStringAsync(key);
            int count = string.IsNullOrEmpty(countString) ? 0 : int.Parse(countString);
            //log ip and time of access
            logger.LogInformation("API accessed at {Time} from IP {IP}", DateTime.UtcNow, key.ToString());
            if (count >= LIMIT)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                return;
            }

            count++;
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(WINDOW_SECONDS)
            };
            await cache.SetStringAsync(key, count.ToString(), options);

            await next(context);
        }
    }
}
