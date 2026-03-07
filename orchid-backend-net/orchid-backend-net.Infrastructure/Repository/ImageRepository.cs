using AutoMapper;
using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class ImageRepository(OrchidDbContext context, IMapper mapper) : RepositoryBase<Domain.Entities.Imgs, Domain.Entities.Imgs, OrchidDbContext>(context, mapper), Domain.IRepositories.IImageRepository
    {
        public Task<List<Imgs>> GetImagesByTargetAsync(string targetId, ImageTargetType targetType, CancellationToken cancellationToken)
        {
            return context.Imgs
                .Where(img => img.TargetId == targetId
                && img.TargetType == targetType)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> SetOldImagesNotNewest(string targetId, ImageTargetType targetType, CancellationToken cancellationToken)
        {
            var oldImages = context.Imgs
                .Where(img => img.TargetId == targetId
                && img.TargetType == targetType
                && img.IsNewest);
            foreach (var img in oldImages)
            {
                img.IsNewest = false;
            }
            return true;
        }
    }
}
