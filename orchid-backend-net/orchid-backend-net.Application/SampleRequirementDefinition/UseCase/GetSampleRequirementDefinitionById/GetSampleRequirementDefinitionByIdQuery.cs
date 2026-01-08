using MediatR;
using orchid_backend_net.Application.SampleRequirementDefinition.Dto;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.SampleRequirementDefinition.UseCase.GetSampleRequirementDefinitionById
{
    public record GetSampleRequirementDefinitionByIdQuery(string Id) : IRequest<SampleRequirementDefinitionDto>;
    internal class GetSampleRequirementDefinitionByIdQueryHandler(
        ISampleRequirementDefinitionRepository sampleRequirementDefinitionRepository)
        : IRequestHandler<GetSampleRequirementDefinitionByIdQuery, SampleRequirementDefinitionDto>
    {
        public async Task<SampleRequirementDefinitionDto> Handle(GetSampleRequirementDefinitionByIdQuery request, CancellationToken cancellationToken)
        {
            var sampleReq = await sampleRequirementDefinitionRepository.FindProjectToAsync<SampleRequirementDefinitionDto>(
                queryOptions: q => q.Where(s => s.ID == request.Id),
                cancellationToken);
            if (sampleReq is null)
                throw new NotFoundException("Không tìm thấy sample requirement này.");
            return sampleReq;
        }
    }
}
