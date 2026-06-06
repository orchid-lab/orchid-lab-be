using MediatR;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Disease.Dto;
using orchid_backend_net.Domain.IRepositories;
namespace orchid_backend_net.Application.Disease.UseCase.GetAllDisease
{
    internal class GetAllDiseaseQueryHandler(IDiseaseRepository repo)
        : IRequestHandler<GetAllDiseaseQuery, IPageResult<DiseaseDto>>
    {
        public async Task<IPageResult<DiseaseDto>> Handle(
            GetAllDiseaseQuery request, CancellationToken ct)
        {
            return await repo.FindAllProjectToAsync<DiseaseDto>(
                pageNo: request.PageNo,
                pageSize: request.PageSize,
                queryOptions: q => q.OrderByDescending(d => d.CreatedAt),
                cancellationToken: ct
            );
        }
    }
}