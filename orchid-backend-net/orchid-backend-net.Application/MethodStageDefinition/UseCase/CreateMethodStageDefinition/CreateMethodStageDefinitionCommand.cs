using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MethodStageDefinition.UseCase.CreateMethodStageDefinition
{
    public record class CreateMethodStageDefinitionCommand(string Name, string Description) : IRequest<string>;
    internal class CreateMethodStageDefinitionCommandHandler(IMethodStageDefinitionRepository methodStageDefinitionRepository) : IRequestHandler<CreateMethodStageDefinitionCommand, string>
    {
        public async Task<string> Handle(CreateMethodStageDefinitionCommand request, CancellationToken cancellationToken)
        {
            var isDuplicated = await methodStageDefinitionRepository.AnyAsync(c => c.Name == request.Name, cancellationToken);
            if (isDuplicated)
            {
                throw new DuplicateException("Method Stage Definition with the same name already exists.");
            }

            var methodStageDefinition = new Domain.Entities.MethodStageDefinition
            {
                Name = request.Name,
                Description = request.Description
            };
            methodStageDefinitionRepository.Add(methodStageDefinition);
            return await methodStageDefinitionRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? methodStageDefinition.ID.ToString()
                : throw new Exception("Create Method Stage Definition failed.");
        }
    }


}
