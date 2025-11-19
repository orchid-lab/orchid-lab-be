using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Seedling
{
    public class SeedlingAttributeDTO : IMapFrom<SeedlingAttributes>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        public static SeedlingAttributeDTO Create(string name, string description)
        {
            return new SeedlingAttributeDTO
            {
                Name = name,
                Description = description
            };
        }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<SeedlingAttributes, SeedlingAttributeDTO>()
                .ReverseMap();
        }
    }
}
