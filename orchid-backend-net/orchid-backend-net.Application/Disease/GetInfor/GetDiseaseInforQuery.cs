using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.GetInfor
{
    public class GetDiseaseInforQuery(string id) : IRequest<DiseaseDTO>, IQuery
    {
        public string Id { get; set; } = id;
    }

    internal class GetDiseaseInforQueryHandler(IDiseaseRepository diseaseRepository) : IRequestHandler<GetDiseaseInforQuery, DiseaseDTO>
    {
        public async Task<DiseaseDTO> Handle(GetDiseaseInforQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var disease = await diseaseRepository.FindProjectToAsync<DiseaseDTO>(
                    query => query.Where(disease => disease.ID == request.Id),
                    cancellationToken);
                return disease;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
