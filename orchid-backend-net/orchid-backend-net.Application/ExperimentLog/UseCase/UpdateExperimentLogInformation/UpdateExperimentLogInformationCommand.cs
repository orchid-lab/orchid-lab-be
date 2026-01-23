using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.UpdateExperimentLogInformation
{
    public record UpdateExperimentLogInformationCommand(string Id, string? Name, string? Notes, int? ExpectedSampleCount) : IRequest<string>;

    internal class UpdateExperimentLogInformationCommandHandler(IExperimentLogRepository experimentLogRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UpdateExperimentLogInformationCommand, string>
    {
        public async Task<string> Handle(UpdateExperimentLogInformationCommand request, CancellationToken cancellationToken)
        {
            var experimentLog = await experimentLogRepository.FindAsync(el => el.ID == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Experiment log with ID {request.Id} not found.");
            experimentLog.UpdateInformation(request.Name, request.Notes, request.ExpectedSampleCount);
            experimentLog.UpdatedDate = DateTime.UtcNow;
            experimentLog.UpdatedBy = currentUserService.UserId;
            experimentLogRepository.Update(experimentLog);
            return await experimentLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                    ? experimentLog.ID
                    : "Cập nhật thất bại";
        }
    }
}
