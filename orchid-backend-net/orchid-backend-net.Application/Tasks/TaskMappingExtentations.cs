using AutoMapper;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Tasks
{
    public static class TaskMappingExtentations
    {

        public static TaskDTO MapToTaskDTO(this orchid_backend_net.Domain.Entities.Tasks task, IMapper mapper)
             => mapper.Map<TaskDTO>(task);

        public static List<TaskDTO> MapToTaskDTOList(this IEnumerable<orchid_backend_net.Domain.Entities.Tasks> taskList, IMapper mapper)
            => taskList.Select(x => x.MapToTaskDTO(mapper)).ToList();

    }
}
