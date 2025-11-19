using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Element
{
    public class ElementDTO : IMapFrom<Elements>
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int CurrentInStage {  get; set; }

        public static ElementDTO Create(string id, string name, string description)
        {
            return new ElementDTO
            {
                ID = id,
                Name = name,
                Description = description
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Elements, ElementDTO>()
                .ForMember(dest => dest.CurrentInStage, otp => otp.MapFrom(src => src.ElementInStages.Count))
                .ReverseMap();
        }
    }
}
