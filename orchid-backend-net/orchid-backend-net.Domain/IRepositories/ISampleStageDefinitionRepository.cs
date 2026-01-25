namespace orchid_backend_net.Domain.IRepositories
{
    public interface ISampleStageDefinitionRepository
    {
        Task<IReadOnlyList<int>> GetOrderDefinitionIdsAsync(CancellationToken cancellationToken);
        Task<int> GetFirstStageDefinitionIdAsync(CancellationToken cancellationToken);
    }
}
