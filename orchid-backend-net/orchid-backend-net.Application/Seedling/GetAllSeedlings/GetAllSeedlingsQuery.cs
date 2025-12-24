using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.GetAllSeedlings
{
    public class GetAllSeedlingsQuery : IRequest<PageResult<SeedlingsDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchTerm { get; set; }
        public string? ByParentLocalName { get; set; }
        public string? ByParentScientificName { get; set; }

        public GetAllSeedlingsQuery(int pageNumber, int pageSize, string? searchTerm, string? byParentLocalName, string? byParentScientificName)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchTerm = searchTerm;
            ByParentLocalName = byParentLocalName;
            ByParentScientificName = byParentScientificName;
        }

        public GetAllSeedlingsQuery() { }
    }

    internal class GetAllSeedlingsQueryHandler(ISeedlingRepository seedlingRepository) : IRequestHandler<GetAllSeedlingsQuery, PageResult<SeedlingsDto>>
    {
        public async Task<PageResult<SeedlingsDto>> Handle(GetAllSeedlingsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Seedlings> queryOptions(IQueryable<Seedlings> query)
            {
                if(!string.IsNullOrWhiteSpace(request.SearchTerm))
                    query = query.Where(s => s.LocalName.Contains(request.SearchTerm) || s.ScientificName.Contains(request.SearchTerm));

                if (!string.IsNullOrWhiteSpace(request.ByParentLocalName))
                    query = query.Where(s => 
                        (s.ParentA != null && s.ParentA.LocalName.Contains(request.ByParentLocalName)) 
                        || (s.ParentB != null && s.ParentB.LocalName.Contains(request.ByParentLocalName))
                    );

                if (!string.IsNullOrWhiteSpace(request.ByParentScientificName))
                    query = query.Where(s =>
                        (s.ParentA != null && s.ParentA.ScientificName.Contains(request.ByParentScientificName))
                        || (s.ParentB != null && s.ParentB.ScientificName.Contains(request.ByParentScientificName))
                    );

                return query;
            }
            var list = await seedlingRepository.FindAllProjectToAsync<SeedlingsDto>(
                pageNo: request.PageNumber,
                pageSize: request.PageSize,
                queryOptions: queryOptions,
                cancellationToken: cancellationToken);
            return list.ToAppPageResult();
        }
    }
}
