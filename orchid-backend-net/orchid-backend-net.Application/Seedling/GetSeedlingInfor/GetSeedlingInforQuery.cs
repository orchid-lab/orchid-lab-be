using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.GetSeedlingInfor
{
    public class GetSeedlingInforQuery : IRequest<SeedlingDTO>, IQuery
    {
        public string SeedlingId { get; set; }
        public GetSeedlingInforQuery(string seedlingId)
        {
            this.SeedlingId = seedlingId;
        }
        public GetSeedlingInforQuery()
        {
        }
    }

    internal class GetSeedlingInforQueryHandler(ISeedlingRepository seedlingRepository) : IRequestHandler<GetSeedlingInforQuery, SeedlingDTO>
    {
        public async Task<SeedlingDTO> Handle(GetSeedlingInforQuery request, CancellationToken cancellationToken)
        {
            try
            {
               var seedling = await seedlingRepository.FindProjectToAsync<SeedlingDTO>(
                    query => query.Where(x => x.ID.Equals(request.SeedlingId)),
                    cancellationToken: cancellationToken);
                return seedling;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
