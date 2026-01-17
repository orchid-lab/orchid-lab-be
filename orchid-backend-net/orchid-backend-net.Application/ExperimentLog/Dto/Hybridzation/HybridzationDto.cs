using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.ExperimentLog.Dto.Hybridzation
{
    public class HybridzationDto : IMapFrom<Hybridzations>
    {
        public string Id { get; set; }
        public string ParentAId { get; set; }
        public string ParentALocalName { get; set; }
        public string ParentAScientificName { get; set; }
        public string ParentBId { get; set; }
        public string ParentBLocalName { get; set; }
        public string ParentBScientificName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Hybridzations, HybridzationDto>()
                .ForMember(dest => dest.ParentALocalName,
                opt => opt.MapFrom(src => src.ParentA.LocalName))
                .ForMember(dest => dest.ParentAScientificName,
                opt => opt.MapFrom(src => src.ParentA.ScientificName))
                .ForMember(dest => dest.ParentBLocalName,
                opt => opt.MapFrom(src => src.ParentB.LocalName))
                .ForMember(dest => dest.ParentBScientificName,
                opt => opt.MapFrom(src => src.ParentB.ScientificName));
        }
    }
}
