using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.SampleRequirementDefinition.Dto;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.SampleRequirementDefinition.UseCase.GetAllSampleRequirementDefinition
{
    public record GetAllSampleRequirementDefinitionQuery(int PageNo, int PageSize) : IRequest<PageResult<SampleRequirementDefinitionDto>>;
    internal class GetAllSampleRequirementDefinitionQueryHandler(
        ISampleRequirementDefinitionRepository sampleRequirementDefinitionRepository)
        : IRequestHandler<GetAllSampleRequirementDefinitionQuery, PageResult<SampleRequirementDefinitionDto>>
    {
        public async Task<PageResult<SampleRequirementDefinitionDto>> Handle(GetAllSampleRequirementDefinitionQuery request, CancellationToken cancellationToken)
        {
            var sampleRequirement = await sampleRequirementDefinitionRepository.FindAllProjectToAsync<SampleRequirementDefinitionDto>(
                pageNo: request.PageNo,
                pageSize: request.PageSize,
                null,
                cancellationToken);
            return sampleRequirement.ToAppPageResult();
        }
    }
}
