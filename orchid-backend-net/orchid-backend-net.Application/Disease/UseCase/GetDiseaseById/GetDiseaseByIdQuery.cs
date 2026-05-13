using MediatR;
using orchid_backend_net.Application.Disease.Dto;

namespace orchid_backend_net.Application.Disease.UseCase.GetDiseaseById
{
    public record GetDiseaseByIdQuery(int Id) : IRequest<DiseaseDetailDto>;
}