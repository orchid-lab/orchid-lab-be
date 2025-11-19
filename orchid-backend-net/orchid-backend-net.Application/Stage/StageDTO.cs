using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Element;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Stage
{
    public class StageDTO : IMapFrom<Stages>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DateOfProcessing { get; set; }
        public int Step { get; set; }
        public bool Status { get; set; }
        public List<ElementDTO> ElementDTO { get; set; }
        public StageDTO() { }
        public StageDTO(string name, string description, 
            int dateOfProcessing, int step,
            bool status, List<ElementDTO> elementDTOs, string id)
        {
            Name = name;
            Description = description;
            DateOfProcessing = dateOfProcessing;
            Step = step;
            Status = status;
            ElementDTO = elementDTOs;
            Id = id;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Stages, StageDTO>()
                .ForMember(dest => dest.ElementDTO, opt => opt.MapFrom(src => src.ElementInStages.Select(e => e.Element)))
                .ReverseMap();
        }
    }
}
