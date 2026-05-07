using MediatR;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;

namespace orchid_backend_net.Application.DiseaseIncident.UseCase.ReviewIncident
{
    public record ReviewDiseaseIncidentCommand(
        string IncidentId,
        bool IsConfirmed,   // true = xác nhận bệnh thật, false = AI sai
        string? Note
    ) : IRequest<string>;

    internal class ReviewDiseaseIncidentCommandHandler(
        IDiseaseIncidentRepository diseaseIncidentRepository,
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService,
        INotificationPushService notificationPushService
    ) : IRequestHandler<ReviewDiseaseIncidentCommand, string>
    {
        public async Task<string> Handle(ReviewDiseaseIncidentCommand request, CancellationToken cancellationToken)
        {
            var incident = await diseaseIncidentRepository.FindWithDetailsAsync(request.IncidentId, cancellationToken)
                ?? throw new NotFoundException($"Không tìm thấy sự cố bệnh với ID: {request.IncidentId}");

            if (request.IsConfirmed)
            {
                var technicianId = incident.SampleStage.Samples.ExperimentLog.AssignedTo;
                var sampleName = incident.SampleStage.Samples.Name;
                var diseaseName = incident.Disease.Name;
                var title = "Yêu cầu tiêu hủy mẫu vật";
                var content = $"Mẫu '{sampleName}' được xác nhận nhiễm {diseaseName}. " +
                  "Vui lòng tiến hành tiêu hủy mẫu theo quy trình.";

                var noti = CreateNotificationHelper.CreateForSingleUsers(technicianId, title, content, Domain.Common.Enum.NotificationTargetType.ExperimentLog, incident.SampleStage.Samples.ExperimentLog.ID.ToString());
                notificationRepository.Add(noti);
                await notificationPushService.PushToSingleUserAsync(technicianId, title, content);
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
