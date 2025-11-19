using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.GetAllSeedling
{
    public class GetAllSeedlingQuery : IRequest<PageResult<SeedlingDTO>>, IQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Parent1 { get; set; }
        public string? Parent2 { get; set; }
        public string? SearchTerm { get; set; }
        public bool IncludeDeleted { get; set; } = false;
        public GetAllSeedlingQuery(int pageNumber, int pageSize, string parent1, string parent2,string searchTerm)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Parent1 = parent1;
            this.Parent2 = parent2;
            this.SearchTerm = searchTerm;
        }
        public GetAllSeedlingQuery()
        {
        }
    }

    internal class  GetAllSeedlingQueryHandler(ISeedlingRepository seedlingRepository) : IRequestHandler<GetAllSeedlingQuery, PageResult<SeedlingDTO>>
    {
        public async Task<PageResult<SeedlingDTO>> Handle(GetAllSeedlingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<Seedlings> queryOptions(IQueryable<Seedlings> query)
                {
                    if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                        query = query.Where(x => x.LocalName.Contains(request.SearchTerm));

                    if (!string.IsNullOrWhiteSpace(request.Parent1))
                        query = query.Where(x => x.Parent1.ToLower().Contains(request.Parent1.ToLower()));

                    if (!string.IsNullOrWhiteSpace(request.Parent2))
                        query = query.Where(x => x.Parent2.ToLower().Contains(request.Parent2.ToLower()));
                    return query.OrderBy(x => x.Create_date);
                }
                var seedlings = await seedlingRepository.FindAllProjectToAsync<SeedlingDTO>(
                    pageNo: request.PageNumber,
                    pageSize: request.PageSize,
                    queryOptions: queryOptions,
                    cancellationToken: cancellationToken);
                return seedlings.ToAppPageResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
