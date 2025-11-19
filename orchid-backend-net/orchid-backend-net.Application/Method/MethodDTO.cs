using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Stage;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Method
{
    public class MethodDTO : IMapFrom<Methods>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Status { get; set; }
        public List<StageDTO> Stages { get; set; }
        public MethodDTO Create(string id, string name, string description, string type, bool status, List<StageDTO> stages)
            => new()
            {
                Id = id,
                Description = description,
                Name = name,
                Type = type,
                Status = status,
                Stages = stages
            };
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Methods, MethodDTO>()
                .ForMember(dest => dest.Stages, otp => otp.MapFrom(src => src.Stages))
                .ReverseMap();
        }
    }
}
