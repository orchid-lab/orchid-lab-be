using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class SampleStageDefinitionRepository(OrchidDbContext dbContext) : ISampleStageDefinitionRepository
    {
        public async Task<int> GetFirstStageDefinitionIdAsync(CancellationToken cancellationToken)
        {
            var firstId = await dbContext.SampleStageDefinition
                .AsNoTracking()
                .OrderBy(sd => sd.Order)
                .Select(sd => sd.ID)
                .FirstAsync(cancellationToken);
            if(firstId == 0)
                throw new InvalidOperationException("Không có sample stage definition nào ở đây cả");
            return firstId;
        }

        public async Task<IReadOnlyList<int>> GetOrderDefinitionIdsAsync(CancellationToken cancellationToken)
        {
            return await dbContext.SampleStageDefinition
                .AsNoTracking()
                .OrderBy(sd => sd.Order)
                .Select(sd => sd.ID)
                .ToListAsync(cancellationToken);
        }
    }
}
