using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Domain.IRepositories
{
    public interface IMethodRepository : IEFRepository<Methods, Methods>
    {
        public Task<MethodStages> GetMethodStageByMethodIdAndStageOrderAsync(int id, int currentStageOrder, CancellationToken cancellationToken);
    }
}
