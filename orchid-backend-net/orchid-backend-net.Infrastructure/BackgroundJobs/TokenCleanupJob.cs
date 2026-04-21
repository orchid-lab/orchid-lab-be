using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using orchid_backend_net.Application.Authentication.Cleanup;

namespace orchid_backend_net.Infrastructure.BackgroundJobs
{
    public class TokenCleanupJob
    {
        private readonly ISender _sender;
        private readonly ILogger<TokenCleanupJob> _logger;

        public TokenCleanupJob(ISender sender, ILogger<TokenCleanupJob> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        /// <summary>
        /// Cleanup invalid tokens - runs daily
        /// </summary>
        public async Task ExecuteAsync()
        {
            try
            {
                _logger.LogInformation("Starting token cleanup job at {Time}", DateTime.UtcNow);
                var result = await _sender.Send(new CleanupInvalidTokensCommand());
                _logger.LogInformation("Token cleanup completed: {Result}", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token cleanup job");
                throw; // Hangfire will retry
            }
        }
    }
}