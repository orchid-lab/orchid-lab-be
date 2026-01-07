using MediatR;
using orchid_backend_net.Application.StageDefinitiones.Dto;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.StageDefinitiones.UseCase.GetStageDefinitionById
{
    public class GetStageDefinitionByIdQuery : IRequest<StageDefinitionDto>
    {
        public int StageID { get; set; }
        public GetStageDefinitionByIdQuery(int StageID)
        {
            this.StageID = StageID;
        }
        public GetStageDefinitionByIdQuery()
        {
        }
    }
    internal class GetStageDefinitionByIdQueryHandler(IStageDefinitionRepository stageDefinitionRepository) : IRequestHandler<GetStageDefinitionByIdQuery, StageDefinitionDto>
    {
        public async Task<StageDefinitionDto> Handle(GetStageDefinitionByIdQuery request, CancellationToken cancellationToken)
        {
            var stageDefinition = await stageDefinitionRepository.FindProjectToAsync<StageDefinitionDto>(
            queryOptions: q => q.Where(x => x.ID.Equals(request.StageID)),
            cancellationToken: cancellationToken);
            if (stageDefinition == null)
                throw new NotFoundException($"Không tìm thấy thông tin của giai đoạn này");
            return stageDefinition;
        }
    }
}
