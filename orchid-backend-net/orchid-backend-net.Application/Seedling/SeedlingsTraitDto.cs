using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Seedling
{
    public class SeedlingsTraitDto : IMapFrom<SeedlingsTraits>
    {
        public string Name { get; set; }
        public required decimal Value { get; set; }
        public required string Unit { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SeedlingsTraits, SeedlingsTraitDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Characteristics.Name))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Characteristics.Unit));
        }
    }
}
