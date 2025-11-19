using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TaskAssign.UpdateTaskAssign
{
    public class UpdateTaskAssignCommand(string id, string? technicianId) : IRequest, ICommand
    {
        public string Id { get; set; } = id;
        public string? TechnicianId { get; set; } = technicianId;
    }

    internal class UpdateTaskAssignCommandHandler(ITaskAssignRepository taskAssignRepository) : IRequestHandler<UpdateTaskAssignCommand>
    {
        public async Task Handle(UpdateTaskAssignCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var taskAssign = await taskAssignRepository.FindAsync(x => x.ID.Equals(request.Id), cancellationToken);
                taskAssign.TechnicianID = request.TechnicianId ?? taskAssign.TechnicianID;
                taskAssignRepository.Update(taskAssign);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
