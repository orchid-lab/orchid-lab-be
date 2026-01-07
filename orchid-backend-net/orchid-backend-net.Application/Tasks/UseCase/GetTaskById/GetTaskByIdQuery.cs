using MediatR;
using orchid_backend_net.Application.Tasks.Dto.Task;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UseCase.GetTaskById
{
    public class GetTaskByIdQuery : IRequest<TaskDetailDto>
    {
        public required string Id { get; set; }
        public GetTaskByIdQuery() { }
        public GetTaskByIdQuery(string id)
        {
            Id = id;
        }
    }

    internal class GetTaskByIdQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetTaskByIdQuery, TaskDetailDto>
    {
        public async Task<TaskDetailDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.FindProjectToAsync<TaskDetailDto>(
                queryOptions: query => query.Where(t => t.ID.Equals(request.Id)),
                cancellationToken: cancellationToken);
            if (task is null)
            {
                throw new NotFoundException("Không tìm thấy công việc này.");
            }
            return task;
        }
    }
}
