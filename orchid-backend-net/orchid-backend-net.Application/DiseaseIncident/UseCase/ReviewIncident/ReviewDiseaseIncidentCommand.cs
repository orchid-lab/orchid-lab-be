using MediatR;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Application.Common.Interfaces;

namespace orchid_backend_net.Application.DiseaseIncident.UseCase.ReviewIncident
{
    public record ReviewDiseaseIncidentCommand(
        string IncidentId,
        bool IsConfirmed,   // true = xác nhận bệnh thật, false = AI sai
        string? Note
    ) : IRequest<string>;

    internal class ReviewDiseaseIncidentCommandHandler(
        IDiseaseIncidentRepository diseaseIncidentRepository,
        ICurrentUserService currentUserService
    ) : IRequestHandler<ReviewDiseaseIncidentCommand, string>
    {
        public async Task<string> Handle(ReviewDiseaseIncidentCommand request, CancellationToken cancellationToken)
        {
            var incident = await diseaseIncidentRepository.FindWithDetailsAsync(request.IncidentId, cancellationToken)
                ?? throw new NotFoundException($"Không tìm thấy sự cố bệnh với ID: {request.IncidentId}");

            if (request.IsConfirmed)
            {
                incident.ConfirmByHuman(currentUserService.UserId, request.Note);
            }
            else
            {
                incident.DismissByHuman(currentUserService.UserId, request.Note ?? "");
            }

            await diseaseIncidentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return incident.ID;
        }
    }
}
