using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedureStep;
using orchid_backend_net.Application.SafeProcedure.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.SafeProcedure.UseCase.Update
{
    public record UpdateSafeProcedureCommand(string Id, string? ProcedureName, string? Description, string? ProcedureType, List<UpdateSafeProcedureStepDto>? Steps) : IRequest<string>;
    internal class UpdateSafeProcedureCommandHandler(
        ISafeProcedureRepository safeProcedureRepository,
        ICurrentUserService currentUserService)
        : IRequestHandler<UpdateSafeProcedureCommand, string>
    {
        public async Task<string> Handle(UpdateSafeProcedureCommand request, CancellationToken cancellationToken)
        {
            var safeProc = await safeProcedureRepository.FindAsync(c => c.ID.Equals(request.Id)
            , cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy quy trình an toàn này");

            safeProc.ProcedureName = request.ProcedureName ?? safeProc.ProcedureName;
            safeProc.Description = request.Description ?? safeProc.Description;
            safeProc.ProcedureType = request.ProcedureType ?? safeProc.ProcedureType;
            safeProc.UpdatedBy = currentUserService.UserId;
            safeProc.UpdatedDate = DateTime.UtcNow;
            SafeProcedureHelper.UpdateStepsOfSafeProcedure(safeProc, request.Steps);
            safeProcedureRepository.Update(safeProc);
            return await safeProcedureRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? safeProc.ID.ToString()
                : "Thất bại";
        }
    }
}
