using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedureStep;
using orchid_backend_net.Application.SafeProcedure.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.SafeProcedure.UseCase.Create
{
    public record CreateSafeProcedureCommand(
        string ProcedureName, 
        string ProcedureType, 
        string? Description, 
        List<CreateSafeProcedureStepDto> SafeProcedureStepDto)
        : IRequest<string>;
    internal class CreateSafeProcedureCommandHandler
        (ISafeProcedureRepository safeProcedureRepository,
        ICurrentUserService currentUserService)
        : IRequestHandler<CreateSafeProcedureCommand, string>
    {
        public async Task<string> Handle(CreateSafeProcedureCommand request, CancellationToken cancellationToken)
        {
            var isDuplicated = await safeProcedureRepository.AnyAsync(
                sp => sp.ProcedureName == request.ProcedureName, cancellationToken);
            if (isDuplicated)
                throw new DuplicateException("Cơ chế an toàn này đã có sẵn rồi");

            var safeProcedure = new Domain.Entities.SafeProcedure
            {
                ProcedureName = request.ProcedureName,
                ProcedureType = request.ProcedureType,
                Description = request.Description,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = currentUserService.UserId
            };
            
            SafeProcedureHelper.AddStepsToSafeProcedure(safeProcedure, request.SafeProcedureStepDto);
            safeProcedureRepository.Add(safeProcedure);
            return await safeProcedureRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? safeProcedure.ID.ToString()
                : "Tạo thất bại";
        }
    }
}
