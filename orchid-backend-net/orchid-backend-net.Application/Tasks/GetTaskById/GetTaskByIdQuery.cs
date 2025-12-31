using MediatR;
using orchid_backend_net.Application.Tasks.Dto;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.GetTaskById
{
    public class GetTaskByIdQuery : IRequest<TaskDto>
    {
        public required string Id { get; set; }
    }
    
    internal class GetTaskByIdQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.FindProjectToAsync<TaskDto>(
                queryOptions: query => query.Where(t => t.ID.Equals(request.Id)),
                cancellationToken: cancellationToken);  
            if(task is null)
            {
                throw new NotFoundException("Không tìm thấy công việc này.");
            }
            return task;
        }
    }
}
