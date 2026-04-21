using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Characteristics.Dto
{
    public class CharacteristicDto : IMapFrom<Characteristic>
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Characteristic, CharacteristicDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ID.ToString()));
        }
    }
}
