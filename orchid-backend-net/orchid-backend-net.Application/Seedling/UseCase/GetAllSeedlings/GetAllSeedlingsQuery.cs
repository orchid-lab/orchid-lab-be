using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Seedling.Dto;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.UseCase.GetAllSeedlings
{
    public class GetAllSeedlingsQuery : IRequest<PageResult<SeedlingDto>>
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

    internal class GetAllSeedlingsQueryHandler(ISeedlingRepository seedlingRepository) : IRequestHandler<GetAllSeedlingsQuery, PageResult<SeedlingDto>>
    {
        public async Task<PageResult<SeedlingDto>> Handle(GetAllSeedlingsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Seedlings> queryOptions(IQueryable<Seedlings> query)
            {
                if(!string.IsNullOrWhiteSpace(request.SearchTerm))
                    query = query.Where(s => s.LocalName.ToLower().Contains(request.SearchTerm.ToLower()) || s.ScientificName.Contains(request.SearchTerm.ToLower()));

                if (!string.IsNullOrWhiteSpace(request.ByParentLocalName))
                    query = query.Where(s => 
                        s.ParentA != null && s.ParentA.LocalName.ToLower().Contains(request.ByParentLocalName.ToLower()) 
                    );

                if (!string.IsNullOrWhiteSpace(request.ByParentScientificName))
                    query = query.Where(s =>
                        s.ParentA != null && s.ParentA.ScientificName.ToLower().Contains(request.ByParentScientificName.ToLower())
                    );

                return query.OrderByDescending(s => s.CreatedDate);
            }
            var list = await seedlingRepository.FindAllProjectToAsync<SeedlingDto>(
                pageNo: request.PageNumber,
                pageSize: request.PageSize,
                queryOptions: queryOptions,
                cancellationToken: cancellationToken);
            return list.ToAppPageResult();
        }
    }
}
