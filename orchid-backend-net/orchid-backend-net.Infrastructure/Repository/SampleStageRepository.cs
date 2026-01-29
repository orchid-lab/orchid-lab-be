using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class SampleStageRepository(OrchidDbContext context) : ISampleStageRepository
    {
        public async Task<SampleStage> FindSampleStageById(string id, CancellationToken cancellationToken)
        {
            return await context.SampleStages.SingleOrDefaultAsync(s => s.ID.Equals(id) && s.Status.Equals(SampleStatus.InProgressed), cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy sample stage này");
        }
    }
}
