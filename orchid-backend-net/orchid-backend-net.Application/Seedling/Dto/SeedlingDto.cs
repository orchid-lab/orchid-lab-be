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
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Seedlings, SeedlingDto>();
        }
    }
}
