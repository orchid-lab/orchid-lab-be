using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Seedling
{
    public class SeedlingsDto : IMapFrom<Seedlings>
    {
        public required string Id { get; set; }
        public required string LocalName { get; set; }
        public required string ScientificName { get; set; }
        public string? Description { get; set; }
        public string? ParentALocalName { get; set; }
        public string? ParentAScientificName { get; set; }
        public string? ParentBLocalName { get; set; }
        public string? ParentBScientificName { get; set; }
        public List<SeedlingsTraitDto> Traits { get; set; } = [];
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Seedlings, SeedlingsDto>()
                .ForMember(dest => dest.ParentALocalName, opt => opt.MapFrom(src => src.ParentA != null ? src.ParentA.LocalName : null))
                .ForMember(dest => dest.ParentAScientificName, opt => opt.MapFrom(src => src.ParentA != null ? src.ParentA.ScientificName : null))
                .ForMember(dest => dest.ParentBLocalName, opt => opt.MapFrom(src => src.ParentB != null ? src.ParentB.LocalName : null))
                .ForMember(dest => dest.ParentBScientificName, opt => opt.MapFrom(src => src.ParentB != null ? src.ParentB.ScientificName : null))
                .ForMember(dest => dest.Traits, opt => opt.MapFrom(src => src.SeedlingsTraits));
        }
    }
}
