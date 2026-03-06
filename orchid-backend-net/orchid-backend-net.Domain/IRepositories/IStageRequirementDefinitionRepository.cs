using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Domain.IRepositories
{
    public interface IStageRequirementDefinitionRepository : IEFRepository<StageRequirementDefinition, StageRequirementDefinition>
    {
        Task<StageRequirementDefinition> FindStageRequirementDefinitionById(string id, CancellationToken cancellationToken);
    }
}
