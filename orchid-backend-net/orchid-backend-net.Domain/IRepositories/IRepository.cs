namespace orchid_backend_net.Domain.IRepositories
{
    public interface IRepository<in TDomain>
    {
        void Add(TDomain entity);
        void Update(TDomain entity);
        void Remove(TDomain entity);
        void AddRange(IEnumerable<TDomain> entities);
        void RemoveRange(IEnumerable<TDomain> entities);
        void UpdateRange(IEnumerable<TDomain> entities);
    }
}
