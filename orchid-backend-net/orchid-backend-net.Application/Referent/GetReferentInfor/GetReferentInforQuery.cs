using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Referent.GetReferentInfor
{
    public class GetReferentInforQuery(string id) : IRequest<ReferentDTO>, IQuery
    {
        public string Id { get; set; } = id;
    }

    internal class GetReferentInforQueryHandler(IReferentRepository referentRepository) : IRequestHandler<GetReferentInforQuery, ReferentDTO>
    {
        public async Task<ReferentDTO> Handle(GetReferentInforQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<Referents> queryOptions(IQueryable<Referents> query)
                {
                    query = query.Where(x => x.ID.Equals(request.Id));
                    return query;
                }
                var referents = await referentRepository.FindProjectToAsync<ReferentDTO>(
                    queryOptions, 
                    cancellationToken);
                return referents;
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
