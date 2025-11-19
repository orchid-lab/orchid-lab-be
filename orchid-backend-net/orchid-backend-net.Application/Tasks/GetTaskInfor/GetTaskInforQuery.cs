using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.GetTaskInfor
{
    public class GetTaskInforQuery : IRequest<TaskDTO>, IQuery
    {
        public string ID { get; set; }
        public GetTaskInforQuery(string ID)
        {
            this.ID = ID;
        }
    }

    internal class GetTaskInforQueryHandler(ITaskRepository taskRepository, IUserRepository userRepository) : IRequestHandler<GetTaskInforQuery, TaskDTO>
    {
        public async Task<TaskDTO> Handle(GetTaskInforQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await taskRepository.FindProjectToAsync<TaskDTO>
                    (queryOptions: query => query.Where(x => x.ID.Equals(request.ID) && x.Status != 5), cancellationToken);
                var usersDict = await userRepository.FindAllToDictionaryAsync(x => x.Status, key => key.ID, value => value.Name, cancellationToken);
                if(usersDict.TryGetValue(task.Researcher, out var userName))
                {
                    task.Researcher = userName;
                }
                return task;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
