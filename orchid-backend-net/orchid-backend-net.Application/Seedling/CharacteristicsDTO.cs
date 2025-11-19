using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Seedling
{
    public class CharacteristicsDTO : IMapFrom<Characteristics>
    {
        public decimal Value { get; set; }
        public SeedlingAttributeDTO SeedlingAttribute { get; set; }

        public static CharacteristicsDTO Create(decimal value, SeedlingAttributeDTO seedlingAttribute)
        {
            return new CharacteristicsDTO
            {
                Value = value,
                SeedlingAttribute = seedlingAttribute
            };
        }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Characteristics, CharacteristicsDTO>()
                .ForMember(dest => dest.SeedlingAttribute, opt => opt.MapFrom(src => src.SeedlingAttribute))
                .ReverseMap();
        }
    }
}
