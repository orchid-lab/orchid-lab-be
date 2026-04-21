using MediatR;
using orchid_backend_net.Application.Chemicals.Dto;
using orchid_backend_net.Application.MethodStageDefinition.Dto;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.MethodStageDefinition.UseCase.GetMethodStageDefinitionById
{
    public class GetMethodStageDefinitionByIdQuery : IRequest<MethodStageDefinitionDto>
    {
        public required int Id;
        public GetMethodStageDefinitionByIdQuery(int id)
        {
            Id = id;
        }
        public GetMethodStageDefinitionByIdQuery() 
        {
        }
    }
    internal class GetMethodStageDefinitionByIdQueryHandler(IMethodStageDefinitionRepository methodStageDefinitionRepository) :
        IRequestHandler<GetMethodStageDefinitionByIdQuery, MethodStageDefinitionDto>
    {
        public async Task<MethodStageDefinitionDto> Handle(GetMethodStageDefinitionByIdQuery request, CancellationToken cancellationToken)
        {
            var methodStageDefinition = await methodStageDefinitionRepository.FindProjectToAsync<MethodStageDefinitionDto>(
                queryOptions: q => q.Where(x => x.ID == request.Id),
                cancellationToken);
            return methodStageDefinition;
        }
    }
}
