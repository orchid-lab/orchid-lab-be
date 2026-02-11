using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Sample.Dto.Sample;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.UseCase.GetAll
{
    public record GetAllSampleQuery(
        int PageNo,
        int PageSize, 
        string? ExperimentLogId)
        : IRequest<PageResult<SampleDto>>;

    internal class GetAllSampleQueryHandler(ISampleRepository sampleRepository) : IRequestHandler<GetAllSampleQuery, PageResult<SampleDto>>
    {
        public async Task<PageResult<SampleDto>> Handle(GetAllSampleQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Samples> queryOptions(IQueryable<Samples> query)
            {
                if (!string.IsNullOrWhiteSpace(request.ExperimentLogId))
                    query = query.Where(s => s.ExperimentLogId.Equals(request.ExperimentLogId));
                return query;
            }

            var result = await sampleRepository.FindAllProjectToAsync<SampleDto>(
                pageNo: request.PageNo,
                pageSize: request.PageSize,
                queryOptions: queryOptions,
                cancellationToken);
            return result.ToAppPageResult();
        }
    }
}
