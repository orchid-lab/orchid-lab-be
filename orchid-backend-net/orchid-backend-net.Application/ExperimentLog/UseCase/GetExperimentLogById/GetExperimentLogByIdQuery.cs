using MediatR;
using orchid_backend_net.Application.ExperimentLog.Dto.ExperimentLog;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.GetExperimentLogById
{
    public record GetExperimentLogByIdQuery(string Id) : IRequest<ExperimentLogDetailDto>;
    internal class GetExperimentLogByIdQueryHandler(IExperimentLogRepository experimentLogRepository) : IRequestHandler<GetExperimentLogByIdQuery, ExperimentLogDetailDto>
    {
        public async Task<ExperimentLogDetailDto> Handle(GetExperimentLogByIdQuery request, CancellationToken cancellationToken)
        {
            var el = await experimentLogRepository.FindProjectToAsync<ExperimentLogDetailDto>(
                queryOptions: query => 
                query.Where(
                    el => el.ID == request.Id),
                cancellationToken);
            return el is null ? throw new NotFoundException("Không tìm thấy experiment log này.") : el;
        }
    }
}
