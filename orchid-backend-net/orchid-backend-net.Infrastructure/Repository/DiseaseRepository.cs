using AutoMapper;
using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class DiseaseRepository(OrchidDbContext context, IMapper mapper)
        : RepositoryBase<Disease, Disease, OrchidDbContext>(context, mapper),
          IDiseaseRepository
    {
        private readonly OrchidDbContext _context = context;

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
            => await _context.Set<Disease>()
                .AnyAsync(d => d.Name.ToLower() == name.ToLower(), ct);

        public async Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default)
            => await _context.Set<Disease>()
                .AnyAsync(d => d.Code.ToLower() == code.ToLower(), ct);
    }
}