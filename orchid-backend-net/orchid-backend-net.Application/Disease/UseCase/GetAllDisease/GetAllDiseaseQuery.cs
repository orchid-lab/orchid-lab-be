using MediatR;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Disease.Dto;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.UseCase.GetAllDisease
{
    public record GetAllDiseaseQuery(int PageNo, int PageSize)
        : IRequest<IPageResult<DiseaseDto>>;
}