using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.MethodStageDefinition.Dto;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MethodStageDefinition.UseCase.GetAllMethodStageDefinition
{
    public class GetAllMethodStageDefinitionQuery : IRequest<PageResult<MethodStageDefinitionDto>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        GetAllMethodStageDefinitionQuery(int pageNo, int pageSize)
        {
            PageNo = pageNo;
            PageSize = pageSize;
        }
        GetAllMethodStageDefinitionQuery()
        {

        }
    }
    internal class GetAllMethodStageDefinitionQueryHandler(IMethodStageDefinitionRepository methodStageDefinitionRepository) : IRequestHandler<GetAllMethodStageDefinitionQuery, PageResult<MethodStageDefinitionDto>>
    {
        public async Task<PageResult<MethodStageDefinitionDto>> Handle(GetAllMethodStageDefinitionQuery request, CancellationToken cancellationToken)
        {
            var methodStageDefinition = await methodStageDefinitionRepository.FindAllProjectToAsync<MethodStageDefinitionDto>(
                request.PageNo,
                request.PageSize,
                null,
                cancellationToken);
            return methodStageDefinition.ToAppPageResult();
        }
    }
}
