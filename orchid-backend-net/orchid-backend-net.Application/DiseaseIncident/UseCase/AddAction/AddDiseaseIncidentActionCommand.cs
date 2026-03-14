using MediatR;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Domain.Common.Exceptions;

namespace orchid_backend_net.Application.DiseaseIncident.UseCase.AddAction
{
    public record AddDiseaseIncidentActionCommand(
        string IncidentId,
        string ActionDescription,
        string? Result
    ) : IRequest<string>;

    internal class AddDiseaseIncidentActionCommandHandler(
        IDiseaseIncidentRepository diseaseIncidentRepository
    ) : IRequestHandler<AddDiseaseIncidentActionCommand, string>
    {
        public async Task<string> Handle(AddDiseaseIncidentActionCommand request, CancellationToken cancellationToken)
        {
            var incident = await diseaseIncidentRepository.FindWithDetailsAsync(request.IncidentId, cancellationToken)
                ?? throw new NotFoundException($"Không tìm thấy sự cố bệnh với ID: {request.IncidentId}");

            incident.AddAction(request.ActionDescription, "researcher");
            var sample = incident.SampleStage?.Samples;
            if (sample != null)
            {
                sample.CancelBecauseOfDisease(request.ActionDescription);
            }
            await diseaseIncidentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return incident.ID;
        }
    }
}
