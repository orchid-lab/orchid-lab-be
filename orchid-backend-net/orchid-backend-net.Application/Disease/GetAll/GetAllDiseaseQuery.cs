using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.GetAll
{
    public class GetAllDiseaseQuery(int pageNo, int pageSize, string? searchTerm, decimal? minRate, bool? isActive) : IRequest<PageResult<DiseaseDTO>>, IQuery
    {
        public int PageNumber { get; set; } = pageNo;
        public int PageSize { get; set; } = pageSize;
        public string? SearchTerm { get; set; } = searchTerm;   
        public decimal? MinInfectedRate { get; set; } = minRate;
        public bool? IsActive { get; set; } = isActive;
    }

    internal class GetAllDiseaseQueryHandler(IDiseaseRepository diseaseRepository) : IRequestHandler<GetAllDiseaseQuery, PageResult<DiseaseDTO>>
    {

        public async Task<PageResult<DiseaseDTO>> Handle(GetAllDiseaseQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<Diseases> queryOptions(IQueryable<Diseases> query)
                {
                    query = query.Where(x => x.Status);
                    if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                        query = query.Where(x => x.Name.Contains(request.SearchTerm));
                    if (request.MinInfectedRate.HasValue)
                        query = query.Where(x => x.InfectedRate >= request.MinInfectedRate.Value);
                    if(request.IsActive.HasValue)
                        query = query.Where(x => x.Status == request.IsActive.Value);
                    return query;
                }
                var diseases = await diseaseRepository.FindAllProjectToAsync<DiseaseDTO>(
                    pageNo: request.PageNumber,
                    pageSize: request.PageSize,
                    queryOptions: queryOptions,
                    cancellationToken: cancellationToken);
                return diseases.ToAppPageResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
