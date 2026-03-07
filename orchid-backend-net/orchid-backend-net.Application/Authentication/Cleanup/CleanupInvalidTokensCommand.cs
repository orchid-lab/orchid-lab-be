using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Authentication.Cleanup
{
    /// <summary>
    /// Clean up tokens for users that no longer exist in database
    /// Use this after database reset/migration
    /// </summary>
    public class CleanupInvalidTokensCommand : IRequest<string>, ICommand
    {
    }

    internal class CleanupInvalidTokensCommandHandler(
        IUserRepository userRepository,
        ICacheService cacheService) : IRequestHandler<CleanupInvalidTokensCommand, string>
    {
        public async Task<string> Handle(CleanupInvalidTokensCommand request, CancellationToken cancellationToken)
        {
            // Get all users from database
            var allUsers = await userRepository.FindAllAsync(cancellationToken);
            var validUserIds = allUsers.Select(u => u.ID).ToHashSet();

            // This would require ICacheService to support pattern matching
            // Or you could manually track all refresh tokens in a separate table

            // For now, set all users' refresh tokens to null if they don't have valid Redis entry
            var usersWithTokens = allUsers.Where(u => !string.IsNullOrEmpty(u.RefreshToken)).ToList();
            
            int cleanedCount = 0;
            foreach (var user in usersWithTokens)
            {
                var redisKey = $"auth:refresh_token:{user.RefreshToken!.Trim().ToLowerInvariant()}";
                var userId = await cacheService.GetAsync(redisKey);
                
                // If Redis doesn't have this token, clean it from DB
                if (string.IsNullOrEmpty(userId) || userId != user.ID)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = null;
                    cleanedCount++;
                }
            }

            if (cleanedCount > 0)
            {
                await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }

            return $"Đã dọn dẹp {cleanedCount} token không hợp lệ.";
        }
    }
}