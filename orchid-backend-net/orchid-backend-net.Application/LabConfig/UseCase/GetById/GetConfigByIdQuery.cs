using MediatR;
using orchid_backend_net.Application.LabConfig.Dto.LabConfig;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabConfig.UseCase.GetById
{
    public record GetConfigByIdQuery(string Id) : IRequest<ConfigDto>;
    internal class GetConfigByIdQueryHandler(IConfigRepository configRepository) : IRequestHandler<GetConfigByIdQuery, ConfigDto>
    {
        public async Task<ConfigDto> Handle(GetConfigByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await configRepository.FindProjectToAsync<ConfigDto>(
                queryOptions: q => q.Where(c => c.ID.Equals(request.Id)),
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy");
            return result;
        }
    }
}
