using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;

namespace orchid_backend_net.Application.Chemicals.Dto
{
    public class ChemicalDto : IMapFrom<Domain.Entities.Chemicals>
    {
        public required string Name { get; set; }
        public required string Category { get; set; }
        public string? Description { get; set; }
        public required string ConcentrationUnit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Chemicals, ChemicalDto>();
        }
    }
}
