using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Materials.Dto;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Materials.UseCase.GetAllMaterials
{
    public class GetAllMaterialsQuery : IRequest<PageResult<MaterialDto>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? CategoryName { get; set; }

        public GetAllMaterialsQuery(int pageNo, int pageSize, string? categoryName)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            CategoryName = categoryName;
        }

        public GetAllMaterialsQuery()
        {
        }
    }

    internal class GetAllMaterialQueryHandler(IMaterialRepository materialRepository) : IRequestHandler<GetAllMaterialsQuery, PageResult<MaterialDto>>
    {
        public async Task<PageResult<MaterialDto>> Handle(GetAllMaterialsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.Materials> queryOptions(IQueryable<Domain.Entities.Materials> query)
            {
                if (!string.IsNullOrWhiteSpace(request.CategoryName))
                    query = query.Where(c => c.Category.ToLower().Contains(request.CategoryName.ToLower()));
                return query;
            }

            var material = await materialRepository.FindAllProjectToAsync<MaterialDto>(
                request.PageNo,
                request.PageSize,
                queryOptions,
                cancellationToken);
            return material.ToAppPageResult();
        }
    }
}
