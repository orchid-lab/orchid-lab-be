using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Common.Enum;

namespace orchid_backend_net.Application.MonitoringLog.Dto.MonitoringLog
{
    public class MonitoringLogDto : IMapFrom<Domain.Entities.MonitoringLogs>
    {
        public required string Id { get; set; }
        public required string CreatedBy { get; set; }
        public required DateOnly CreatedDate { get; set; }
        public string SampleName { get; set; } = null!;
        public MonitoringLogStatus Status { get; set; }
        public bool IsNewest { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.MonitoringLogs, MonitoringLogDto>()
                .ForMember(dest => dest.SampleName, opt => opt.MapFrom(src => src.SampleStage.Samples.Name))
                .ForMember(dest  => dest.CreatedDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CreatedDate)));
        }
    }
}
