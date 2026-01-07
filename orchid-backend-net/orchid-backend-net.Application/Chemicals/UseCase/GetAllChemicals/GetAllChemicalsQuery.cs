using MediatR;
using orchid_backend_net.Application.Chemicals.Dto;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Chemicals.UseCase.GetAllChemicals
{
    public class GetAllChemicalsQuery : IRequest<PageResult<ChemicalDto>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? CategoryName { get; set; }


        public GetAllChemicalsQuery(int pageNo, int pageSize, string? categoryName)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            CategoryName = categoryName;
        }

        public GetAllChemicalsQuery()
        {
        }
    }

    internal class GetAllChemicalsQueryHandler(IChemicalsRepository chemicalsRepository) : IRequestHandler<GetAllChemicalsQuery, PageResult<ChemicalDto>>
    {
        public async Task<PageResult<ChemicalDto>> Handle(GetAllChemicalsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.Chemicals> queryOptions(IQueryable<Domain.Entities.Chemicals> query)
            {
                if (!string.IsNullOrWhiteSpace(request.CategoryName))
                    query = query.Where(c => c.Category.Contains(request.CategoryName));
                return query;
            }

            var chemicals = await chemicalsRepository.FindAllProjectToAsync<ChemicalDto>(
                request.PageNo,
                request.PageSize,
                queryOptions,
                cancellationToken);
            return chemicals.ToAppPageResult();
        }
    }
}
