using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Domain.IRepositories
{
    /// <summary>
    /// Base interface for repositories that need to query entities with images
    /// </summary>
    public interface IEntityWithImagesRepository
    {
        Task<List<Imgs>> GetImagesByTargetAsync(string targetId, ImageTargetType targetType, CancellationToken cancellationToken = default);
        Task<Imgs?> GetLatestImageByTargetAsync(string targetId, ImageTargetType targetType, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Batch load latest images for multiple targets (optimized for N+1 queries)
        /// </summary>
        Task<List<Imgs>> GetLatestImagesByTargetsAsync(IEnumerable<string> targetIds, CancellationToken cancellationToken = default);
    }
}