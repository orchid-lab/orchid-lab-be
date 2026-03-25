using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.MonitoringLog.Dto.LogDetail;
using orchid_backend_net.Application.Sample.Dto.SampleStageDefinition;
using orchid_backend_net.Domain.Common.Enum;

namespace orchid_backend_net.Application.Sample.Dto.SampleStage
{
    public class SampleStageDto : IMapFrom<Domain.Entities.SampleStage>
    {
        public required string Id { get; set; }
        public DateOnly StartAt { get; set; }
        public SampleStatus Status { get; set; }
        public required SampleStageDefinitionDto SampleStageDefinition { get; set; }
        public List<LogDetailDto> LogDetailDtos { get; set; } = new();
        public string? LatestImageUrl { get; set; } 
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.SampleStage, SampleStageDto>()
                .ForMember(dest => dest.StartAt,
                    opt => opt.MapFrom(
                        src => src.StartedAt))
                .ForMember(dest => dest.LogDetailDtos,
                    opt => opt.MapFrom(
                        src => src.MonitoringLogs
                            .Where(m => m.IsNewest && m.Status == MonitoringLogStatus.Approved)
                            .SelectMany(m => m.LogDetails)
                    ))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.LatestImageUrl, opt => opt.Ignore());
        }
    }
}
