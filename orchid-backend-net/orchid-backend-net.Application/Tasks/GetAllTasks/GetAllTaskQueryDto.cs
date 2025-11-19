using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;

namespace orchid_backend_net.Application.Tasks.GetAllTasks
{
    public class GetAllTaskQueryDto : IMapFrom<Domain.Entities.Tasks>
    {
        public string ID { get; set; }
        public string Researcher { get; set; }
        public string ExperimentLogName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public DateTime Create_at { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Tasks, GetAllTaskQueryDto>()
                .ForMember(dest => dest.ExperimentLogName, opt => opt.MapFrom(src =>
                    src.Linkeds.FirstOrDefault(linked => linked.TaskID.Equals(src.ID)).ExperimentLog.Name));
        }
    }
}
