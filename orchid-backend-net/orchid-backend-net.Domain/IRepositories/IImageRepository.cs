namespace orchid_backend_net.Domain.IRepositories
{
    public interface IImageRepository : IEFRepository<Entities.Imgs, Entities.Imgs>
    {
        Task<List<Entities.Imgs>> GetImagesByTargetAsync(string targetId, Common.Enum.ImageTargetType targetType, CancellationToken cancellationToken);
    }
}
