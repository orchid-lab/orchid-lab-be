using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.StageDefinitiones.Dto;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.StageDefinitiones.UseCase.GetAllStageDefinition
{
    public class GetAllStageDefinitionQuery : IRequest<PageResult<StageDefinitionDto>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }

        public GetAllStageDefinitionQuery(int pageNo, int pageSize)
        {
            PageNo = pageNo;
            PageSize = pageSize;
        }
        public GetAllStageDefinitionQuery()
        {
        }
    }

    internal class GetAllStageDefinitionQueryHandler(IStageDefinitionRepository stageDefinitionRepository) : IRequestHandler<GetAllStageDefinitionQuery, PageResult<StageDefinitionDto>>
    {
        public async Task<PageResult<StageDefinitionDto>> Handle(GetAllStageDefinitionQuery request, CancellationToken cancellationToken)
        => (await stageDefinitionRepository.FindAllProjectToAsync<StageDefinitionDto>(
            pageNo: request.PageNo,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken)).ToAppPageResult();
    }
}
