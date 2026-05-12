using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using DiseaseEntity = orchid_backend_net.Domain.Entities.Disease;

namespace orchid_backend_net.Application.MonitoringLog.Dto.Disease
{
    public class DiseaseDto : IMapFrom<DiseaseEntity>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? OnnxClassName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DiseaseEntity, DiseaseDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ID));
        }
    }
}