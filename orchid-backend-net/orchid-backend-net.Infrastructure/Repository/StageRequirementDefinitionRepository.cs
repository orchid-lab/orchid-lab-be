using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class StageRequirementDefinitionRepository(OrchidDbContext context) : IStageRequirementDefinitionRepository

    {
        public async Task<StageRequirementDefinition> FindStageRequirementDefinitionById(string id, CancellationToken cancellationToken)
        {
            return await context.StageRequirementDefinitions.SingleOrDefaultAsync(s => s.ID.Equals(id), cancellationToken)
                ?? throw new NotFoundException("Không thấy quy cách này");
        }
    }
}
