using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedure;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.SafeProcedure.UseCase.GetAll
{
    public record GetAllSafeProcedureQuery(int PageNo, int PageSize, string NameSearchTerm) : IRequest<PageResult<SafeProcedureDto>>;
    internal class GetAllSafeProcedureQueryHandler(ISafeProcedureRepository safeProcedureRepository) : IRequestHandler<GetAllSafeProcedureQuery, PageResult<SafeProcedureDto>>
    {
        public async Task<PageResult<SafeProcedureDto>> Handle(GetAllSafeProcedureQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.SafeProcedure> queryOptions(IQueryable<Domain.Entities.SafeProcedure> query)
            {
                if (!string.IsNullOrWhiteSpace(request.NameSearchTerm))
                {
                    query = query.Where(sp => sp.ProcedureName.Contains(request.NameSearchTerm));
                }
                return query;
            }

            var result = await safeProcedureRepository.FindAllProjectToAsync<SafeProcedureDto>(
                request.PageNo,
                request.PageSize,
                queryOptions,
                cancellationToken
            );

            return result.ToAppPageResult();
        }
    }
}
