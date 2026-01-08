using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Seedling.Dto
{
    public class SeedlingDto : IMapFrom<Seedlings>
    {
        public required string Id { get; set; }
        public required string LocalName { get; set; }
        public required string ScientificName { get; set; }
        public string? Description { get; set; }
        public string? ParentALocalName { get; set; }
        public string? ParentAScientificName { get; set; }
        public string? ParentBLocalName { get; set; }
        public string? ParentBScientificName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Seedlings, SeedlingDto>();
        }
    }
}
