using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.GetAllSample
{
    public class GetAllSampleQuery : IRequest<PageResult<SampleDTO>>, IQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? ExperimentLogId { get; set; }
        public GetAllSampleQuery(int pagenumber, int pagesize, string? experimentLogId)
        {
            this.PageNumber = pagenumber;
            this.PageSize = pagesize;
            ExperimentLogId = experimentLogId;
        }
        public GetAllSampleQuery() { }
    }

    internal class GetAllSampleQueryHandler(ISampleRepository sampleRepository, IMapper mapper) : IRequestHandler<GetAllSampleQuery, PageResult<SampleDTO>>
    {

        public async Task<PageResult<SampleDTO>> Handle(GetAllSampleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<Domain.Entities.Samples> queryOptions(IQueryable<Domain.Entities.Samples> query)
                {
                    //query = query.Where(x => x.Status != 2);
                    if (!string.IsNullOrWhiteSpace(request.ExperimentLogId))
                        query = query.Where(x => x.Linkeds.Any(linkeds => linkeds.ExperimentLogID.Equals(request.ExperimentLogId)));
                    return query.OrderBy(x => x.Linkeds.FirstOrDefault(t => t.SampleID.Equals(x.ID)).ExperimentLogID);
                }

                var samples = await sampleRepository.FindAllProjectToAsync<SampleDTO>(
                        pageNo: request.PageNumber,
                        pageSize: request.PageSize,
                        queryOptions: queryOptions,
                        cancellationToken: cancellationToken);
                return samples.ToAppPageResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
