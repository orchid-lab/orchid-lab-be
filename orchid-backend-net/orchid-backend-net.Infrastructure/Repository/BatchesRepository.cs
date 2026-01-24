using AutoMapper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class BatchesRepository(OrchidDbContext dbContext, IMapper mapper) : RepositoryBase<Batches, Batches, OrchidDbContext>(dbContext, mapper), IBatchesRepository
    {
        public async Task<Batches> GetByIdAsync(int id, CancellationToken cancellationToken)
            => await FindAsync(b => b.ID == id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy batch này");
    }
}
