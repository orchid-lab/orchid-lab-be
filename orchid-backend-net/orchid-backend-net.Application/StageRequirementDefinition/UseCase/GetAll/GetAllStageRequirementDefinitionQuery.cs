using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.StageRequirementDefinition.Dto.StageRequirementDefinitionDto;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.StageRequirementDefinition.UseCase.GetAll
{
    public record GetAllStageRequirementDefinitionQuery(int PageNo, int PageSize, string? SampleStageId) : IRequest<PageResult<StageRequirementDefinitionDto>>;
    internal class GetAllStageRequirementDefinitionQueryHandler(
        IStageRequirementDefinitionRepository stageRequirementDefinitionRepository)
        : IRequestHandler<GetAllStageRequirementDefinitionQuery, PageResult<StageRequirementDefinitionDto>>
    {
        public async Task<PageResult<StageRequirementDefinitionDto>> Handle(GetAllStageRequirementDefinitionQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.StageRequirementDefinition> queryOptions(IQueryable<Domain.Entities.StageRequirementDefinition> query)
            {
                if (!string.IsNullOrEmpty(request.SampleStageId))
                {
                    query = query.Where(srd =>
                        srd.SampleStage.ID.Equals(request.SampleStageId));
                }
                return query;
            }

            var result = await stageRequirementDefinitionRepository.FindAllProjectToAsync<StageRequirementDefinitionDto>(
                pageNo: request.PageNo,
                pageSize: request.PageSize,
                queryOptions: queryOptions,
                cancellationToken: cancellationToken);

            return result.ToAppPageResult();
        }
    }
}
