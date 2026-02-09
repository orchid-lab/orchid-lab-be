using MediatR;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.SafeProcedure.UseCase.Create
{
    public record CreateSafeProcedureCommand(string ProcedureName, int StepNumber, string Description)
        : IRequest<string>;
    internal class CreateSafeProcedureCommandHandler
        (ISafeProcedureRepository safeProcedureRepository)
        : IRequestHandler<CreateSafeProcedureCommand, string>
    {
        public Task<string> Handle(CreateSafeProcedureCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
