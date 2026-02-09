using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.LabConfig.Dto.LabConfig;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabConfig.UseCase.GetAll
{
    public record GetAllConfigLabQuery(int PageNo, int PageSize) : IRequest<PageResult<ConfigDto>>;
    internal class GetConfigLabQueryHandler(IConfigRepository configRepository) : IRequestHandler<GetAllConfigLabQuery, PageResult<ConfigDto>>
    {
        public async Task<PageResult<ConfigDto>> Handle(GetAllConfigLabQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Config> query(IQueryable<Config> q)
            {
                return q;
            }

            var result = await configRepository.FindAllProjectToAsync<ConfigDto>(
                request.PageNo,
                request.PageSize,
                query,
                cancellationToken);
            return result.ToAppPageResult();
        }
    }
}
