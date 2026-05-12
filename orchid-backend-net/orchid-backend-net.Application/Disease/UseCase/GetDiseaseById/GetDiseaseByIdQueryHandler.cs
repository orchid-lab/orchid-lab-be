using MediatR;
using orchid_backend_net.Application.Disease.Dto;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.UseCase.GetDiseaseById
{
    internal class GetDiseaseByIdQueryHandler(IDiseaseRepository repo)
        : IRequestHandler<GetDiseaseByIdQuery, DiseaseDetailDto>
    {
        public async Task<DiseaseDetailDto> Handle(
            GetDiseaseByIdQuery request, CancellationToken ct)
        {
            return await repo.FindProjectToAsync<DiseaseDetailDto>(
                q => q.Where(d => d.ID == request.Id),
                ct
            ) ?? throw new NotFoundException($"Không tìm thấy bệnh với id {request.Id}");
        }
    }
}