using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.CreateExperimentLog
{
    public record CreateExperimentLogCommand(
        int MethodId,
        int BatchesId,
        string ParentAId,
        string? ParentBId,
        string Name,
        int ExpectedSampleCount,
        string AssignedToTechnicianId) : IRequest<string>;

    internal class CreateExperimentLogCommandHandler(
        IExperimentLogRepository experimentLogRepository,
         IMethodRepository methodRepository,
         ISeedlingRepository seedlingRepository,
         IUserRepository userRepository,
         IBatchesRepository batchesRepository,
         ICurrentUserService currentUserService)
        : IRequestHandler<CreateExperimentLogCommand, string>
    {
        public async Task<string> Handle(CreateExperimentLogCommand request, CancellationToken cancellationToken)
        {
            var method = await methodRepository.FindAsync(m => m.ID == request.MethodId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy method này.");

            var batch = await batchesRepository.FindAsync(b => b.ID == request.BatchesId && !b.IsBatching, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy batch này.");

            var parentA = await seedlingRepository.FindAsync(pA => pA.ID == request.ParentAId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy seedling này");

            Seedlings? parentB = null;
            if (!string.IsNullOrEmpty(request.ParentBId))
            {
                parentB = await seedlingRepository.FindAsync(
                    pB => pB.ID == request.ParentBId, cancellationToken)
                    ?? throw new NotFoundException("Không tìm thấy seedling ParentB");
            }
            var technicianAssigned = await userRepository.FindAsync(u => u.ID == request.AssignedToTechnicianId  && u.RoleID == 3, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy technician");

            var isDuplicatedExperimentLogName = await experimentLogRepository.AnyAsync(
                el => el.Name == request.Name, cancellationToken);
            if (isDuplicatedExperimentLogName)
            {
                throw new DuplicateException("Experiment Log này đã bị trùng");
            }

            var hybrid = new Hybridzations()
            {
                ParentAId = parentA.ID,
                ParentBId = parentB?.ID,
            };

            var eL = new ExperimentLogs()
            {
                MethodId = method.ID,
                BatchId = batch.ID,
                Hybridzations = hybrid,
                HybridzationId = hybrid.ID,
                Name = request.Name,
                ExpectedSampleCount = request.ExpectedSampleCount,
                AssignedTo = technicianAssigned.ID,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = currentUserService.UserId,
                Status = Domain.Common.Enum.ExperimentLogStatus.Created,
            };

            experimentLogRepository.Add(eL);
            return await experimentLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Tạo thành công"
                : "Tạo thất bại";
        }
    }
}
