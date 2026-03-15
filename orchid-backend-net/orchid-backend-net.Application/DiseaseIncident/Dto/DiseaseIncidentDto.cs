using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Common.Enum;

namespace orchid_backend_net.Application.DiseaseIncident.Dto
{
    public class DiseaseIncidentDto : IMapFrom<Domain.Entities.DiseaseIncident>
    {
        public string Id { get; set; } = default!;
        public string SampleStageId { get; set; } = default!;
        public string DiseaseName { get; set; } = default!;
        public DiseaseIncidentStatus Status { get; set; }
        public decimal AIConfidence { get; set; }
        public string? ReviewNote { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.DiseaseIncident, DiseaseIncidentDto>()
                .ForMember(dest => dest.DiseaseName, opt => opt.MapFrom(src => src.Disease != null ? src.Disease.Name : string.Empty));
        }
    }
}
