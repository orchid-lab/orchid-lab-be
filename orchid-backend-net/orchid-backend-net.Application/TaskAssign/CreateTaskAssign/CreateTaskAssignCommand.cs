using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;
using System.Text.Json.Serialization;

namespace orchid_backend_net.Application.TaskAssign.CreateTaskAssign
{
    public class CreateTaskAssignCommand(string technicianId) : IRequest, ICommand
    {
        [JsonIgnore]
        public string? TaskId { get; set; }
        public string TechnicianId { get; set; } = technicianId;
    }

    internal class CreateTaskAssignCommandHandler(ITaskAssignRepository taskAssignRepository) : IRequestHandler<CreateTaskAssignCommand>
    {
        public async Task Handle(CreateTaskAssignCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Domain.Entities.TasksAssign assign = new()
                {
                    TaskID = request.TaskId,
                    TechnicianID = request.TechnicianId,
                    Status = true,
                };
                taskAssignRepository.Add(assign);
            }
            catch (Exception ex) 
            {
                throw new Exception($"{ex.Message}"); 
            }
        }
    }
}
