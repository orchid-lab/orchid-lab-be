using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.ExperimentLog.Helper.CreateExperimentLogHelperInjection;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Common.Interfaces;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.CreateExperimentLog
{
    public record CreateExperimentLogCommand(
        int MethodId,
        int BatchesId,
        string ParentAId,
        string Name,
        int ExpectedSampleCount,
        string AssignedToTechnicianId,
        DateOnly StartDate,
        DateOnly ExpectedEndDate,
        string? Objective // thêm field mới
    ) : IRequest<string>, ICommand;

    internal class CreateExperimentLogCommandHandler(
        CreateExperimentLogRepositories repo,
        CreateExperimentLogServices services,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateExperimentLogCommand, string>
    {
        public async Task<string> Handle(CreateExperimentLogCommand request, CancellationToken cancellationToken)
        {
            var method = await repo.MethodRepository.FindAsync(m => m.ID == request.MethodId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy method này.");

            var batch = await repo.BatchesRepository.FindAsync(b => b.ID == request.BatchesId && b.Status == BatchStatus.Ready, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy batch này.");

            var parent = await repo.SeedlingRepository.FindAsync(pA => pA.ID == request.ParentAId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy seedling này");

            var technicianAssigned = await repo.UserRepository.FindAsync(u => u.ID == request.AssignedToTechnicianId && u.RoleID == 3, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy technician");

            var isDuplicatedExperimentLogName = await repo.ExperimentLogRepository.AnyAsync(
                el => el.Name == request.Name, cancellationToken);
            if (isDuplicatedExperimentLogName)
            {
                throw new DuplicateException("Experiment Log này đã bị trùng");
            }

            var expectedDurationInDays = method.MethodStages.Sum(ms => ms.DurationsDays);

            var eL = new ExperimentLogs()
            {
                MethodId = method.ID,
                BatchId = batch.ID,
                SeedlingParentId = parent.ID,
                Name = request.Name,
                ExpectedSampleCount = request.ExpectedSampleCount,
                AssignedTo = technicianAssigned.ID,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = services.CurrentUserService.UserId!,
                StartDate = request.StartDate,
                ExpectedEndDate = request.ExpectedEndDate,
                Status = ExperimentLogStatus.Created,
                Objective = request.Objective // set field mới
            };

            repo.ExperimentLogRepository.Add(eL);

            var noti = new Domain.Entities.Notification()
            {
                Title = "Được phân công thí nghiệm mới",
                Content = $"Bạn được phân công thí nghiệm {eL.Name} cho phương pháp {method.Name}",
                UserId = technicianAssigned.ID,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                NotificationTargetType = Domain.Common.Enum.NotificationTargetType.ExperimentLog,
                TargetId = eL.ID.ToString()
            };

            repo.NotificationRepository.Add(noti);

            var saved = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
            if (!saved)
            {
                return "Tạo thất bại";
            }
            await services.PushService.PushToSingleUserAsync(noti.UserId, noti.Title, noti.Content);

            return eL.ID.ToString();
        }
    }
}
