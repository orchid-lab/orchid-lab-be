using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.TaskAssign.UpdateTaskAssign;
using orchid_backend_net.Application.TaskAttribute.CreateTaskAttribute;
using orchid_backend_net.Application.TaskAttribute.UpdateTaskAttribute;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UpdateTask
{
    public class UpdateTaskCommand : IRequest<string>, ICommand
    {
        public string ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<UpdateTaskAttributeCommand>? TaskAttributeUpdate { get; set; }
        public List<CreateTaskAttributeCommand>? TaskAttributeCreate { get; set; }
        public UpdateTaskCommand(string id, string? name, string? description,
            List<UpdateTaskAttributeCommand>? attribute, List<CreateTaskAttributeCommand>? createTaskAttributeCommands)
        {
            ID = id;
            Name = name;
            Description = description;
            TaskAttributeUpdate = attribute;
            TaskAttributeCreate = createTaskAttributeCommands;
        }
    }

    //refactor again base on solid
    internal class UpdateTaskCommandHandler(ITaskRepository taskRepository, ITaskAssignRepository taskAssignRepository,
        ITaskAttributeRepository taskAttributeRepository, ICurrentUserService currentUserService,
        ISender sender) : IRequestHandler<UpdateTaskCommand, string>
    {
        public async Task<string> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await taskRepository.FindAsync(x => x.ID == request.ID, cancellationToken);
                task.Name = request.Name ?? task.Name;
                task.Description = request.Description ?? task.Description;
                task.Update_date = DateTime.UtcNow;
                task.Update_by = currentUserService.UserId;

                //check task attribute list to update
                var taskAttributeList = await taskAttributeRepository.FindAllAsync(x => x.TaskID.Equals(request.ID), cancellationToken);
                if (request.TaskAttributeUpdate != null && request.TaskAttributeUpdate.Count > 0)
                {
                    var commandsToUpdate = request.TaskAttributeUpdate
                        .Where(updateCmd => taskAttributeList.Any(x =>
                            x.ID.Equals(updateCmd.Id) && IsDifferentAttribute(x, updateCmd)));

                    foreach (var command in commandsToUpdate)
                    {
                        await sender.Send(command, cancellationToken);
                    }
                }

                //if researcher created a new task attribute => this will handle the case
                if (request.TaskAttributeCreate != null && request.TaskAttributeCreate.Count > 0)
                {
                    foreach (var createTaskAttributeCommand in request.TaskAttributeCreate)
                    {
                        createTaskAttributeCommand.TaskId = task.ID;
                        await sender.Send(createTaskAttributeCommand, cancellationToken);
                    }
                }

                return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Updated task ID :{request.ID}" : $"Failed update task with ID :{request.ID}";
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex.Message}");
            }
        }

        private bool IsDifferentAttribute(TaskAttributes x, UpdateTaskAttributeCommand command)
            => x.Name != command.Name
            || x.MeasurementUnit != command.MeasurementUnit
            || !x.Value.Equals(command.Value);
    }
}
