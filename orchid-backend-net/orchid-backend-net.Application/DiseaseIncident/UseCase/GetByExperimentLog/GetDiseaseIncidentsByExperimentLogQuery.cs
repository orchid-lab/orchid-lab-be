using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.DiseaseIncident.Dto;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.DiseaseIncident.UseCase.GetByExperimentLog
{
    public record GetDiseaseIncidentsByExperimentLogQuery(
        int PageNo, 
        int PageSize,
        string ExperimentLogId,
        DiseaseIncidentStatus? StatusFilter
    ) : IRequest<PageResult<DiseaseIncidentDto>>;

    

    internal class GetDiseaseIncidentsByExperimentLogQueryHandler(
        IDiseaseIncidentRepository diseaseIncidentRepository
    ) : IRequestHandler<GetDiseaseIncidentsByExperimentLogQuery, PageResult<DiseaseIncidentDto>>
    {
        public async Task<PageResult<DiseaseIncidentDto>> Handle(GetDiseaseIncidentsByExperimentLogQuery request, CancellationToken cancellationToken)
        {

            IQueryable<Domain.Entities.DiseaseIncident> query(IQueryable<Domain.Entities.DiseaseIncident> q)
            {
                q = q.Where(x => x.SampleStage.Samples.ExperimentLogId == request.ExperimentLogId);
                if (request.StatusFilter.HasValue)
                {
                    q = q.Where(x => x.Status == request.StatusFilter.Value);
                }
                return q;
            }

            var result = await diseaseIncidentRepository.FindAllProjectToAsync<DiseaseIncidentDto>(
                request.PageNo,
                request.PageSize,
                query,
                cancellationToken
            );

            return result.ToAppPageResult();
        }
    }
}
