using AutoMapper;
using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class SampleRepository(OrchidDbContext dbContext, IMapper mapper) 
        : RepositoryBase<Samples, Samples, OrchidDbContext>(dbContext, mapper), ISampleRepository
    {
        private readonly OrchidDbContext _dbContext = dbContext;

        public Task<List<Imgs>> GetImagesByTargetAsync(string targetId, ImageTargetType targetType, CancellationToken cancellationToken = default)
        {
            return _dbContext.Imgs
                .Where(img => img.TargetId == targetId && img.TargetType == targetType)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public Task<Imgs?> GetLatestImageByTargetAsync(string targetId, ImageTargetType targetType, CancellationToken cancellationToken = default)
        {
            return _dbContext.Imgs
                .Where(img => img.TargetId == targetId 
                    && img.TargetType == targetType 
                    && img.IsNewest)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Batch load latest images for multiple targets in single query
        /// Supports Sample and SampleStage target types
        /// </summary>
        public Task<List<Imgs>> GetLatestImagesByTargetsAsync(IEnumerable<string> targetIds, CancellationToken cancellationToken = default)
        {
            return _dbContext.Imgs
                .Where(img => targetIds.Contains(img.TargetId)
                    && (img.TargetType == ImageTargetType.Sample || img.TargetType == ImageTargetType.SampleStage)
                    && img.IsNewest)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
