using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;

namespace orchid_backend_net.Application.MonitoringLog.Dto.Disease
{
    public class DiseaseDto : IMapFrom<Domain.Entities.Disease>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Disease, DiseaseDto>();
        }
    }
}
