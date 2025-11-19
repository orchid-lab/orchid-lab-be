using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.TaskAssign.CreateTaskAssign;
using orchid_backend_net.Application.TaskAttribute.CreateTaskAttribute;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.CreateTaskWhenCreateExperimentLog
{
    public class CreateTaskWhenCreateExperimentLogCommand : IRequest<string>, ICommand
    {
        public string ExperimentLogID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public List<CreateTaskAttributeCommand> Attribute { get; set; }
        public List<string> TechnicianID { get; set; }
        public CreateTaskWhenCreateExperimentLogCommand(string name, string description,
            DateTime start_date, DateTime end_date, List<CreateTaskAttributeCommand> attribute,
            List<string> technicianID, string experimentLogID)
        {
            Name = name;
            Description = description;
            Start_date = start_date;
            End_date = end_date;
            Attribute = attribute;
            TechnicianID = technicianID;
            ExperimentLogID = experimentLogID;
        }
    }

    public class CreateTaskWhenCreateExperimentLogHandler(ITaskRepository taskRepository, ICurrentUserService currentUserService, ISender sender) : IRequestHandler<CreateTaskWhenCreateExperimentLogCommand, string>
    {
        public async Task<string> Handle(CreateTaskWhenCreateExperimentLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = new Domain.Entities.Tasks()
                {
                    Name = request.Name,
                    Create_at = DateTime.UtcNow,
                    Start_date = request.Start_date,
                    End_date = request.End_date,
                    Description = request.Description,
                    Researcher = currentUserService.UserId,
                    Status = 0,
                };
                taskRepository.Add(task);

                foreach (var command in request.Attribute)
                {
                    command.TaskId = task.ID;
                    await sender.Send(command, cancellationToken);
                }

                foreach (var technicianID in request.TechnicianID)
                {
                    CreateTaskAssignCommand assignCommand = new(technicianID)
                    {
                        TaskId = task.ID
                    };
                    await sender.Send(assignCommand, cancellationToken);
                }
                return task.ID;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
