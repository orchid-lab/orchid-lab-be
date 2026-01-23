using AutoMapper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class MethodRepository(OrchidDbContext dbContext, IMapper mapper) : RepositoryBase<Methods, Methods, OrchidDbContext>(dbContext, mapper), IMethodRepository
    {
        public async Task<MethodStages> GetMethodByIdAsync(int id, int currentStageOrder, CancellationToken cancellationToken)
        {
            var method = await this.FindAsync(m => m.ID == id, cancellationToken)
                ?? throw new NotFoundException("Không thấy phương pháp này");

            var stage = method.MethodStages
                .SingleOrDefault(ms => ms.Order == currentStageOrder)
                ?? throw new NotFoundException("Không thấy giai đoạn này trong phương pháp");

            return stage;
        }
    }
}
