using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Application.Tests.TestData
{
    public static class CacheTestData
    {
        public const string RawToken = "  ABCDEF123  ";
        public const string NormalizedToken = "abcdef123";
        public const string ExpectedCacheKey = "auth:refresh_token:abcdef123";

        public static RefreshToken CreateValidRefreshToken()
        {
            return new()
            {
                Token = "new-refresh-token",
                Expired = new DateOnly(2025,12,29).ToDateTime(new TimeOnly(0,0))    
            };
        }
    }
}
