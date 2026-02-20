using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Images.Dto.Img;
using orchid_backend_net.Application.MonitoringLog.Dto.AnalyticResult;
using orchid_backend_net.Application.MonitoringLog.Dto.LogDetail;
using orchid_backend_net.Domain.Common.Enum;

namespace orchid_backend_net.Application.MonitoringLog.Dto.MonitoringLog
{
    public class MonitoringLogDetailDto : IMapFrom<Domain.Entities.MonitoringLogs>
    {
        public required string Id { get; set; }
        public string Name { get; set; } = null!;
        public required string CreatedBy { get; set; }
        public required DateOnly CreatedDate { get; set; }
        public string SampleName { get; set; } = null!;
        public string SampleStageDefinitionName { get; set; } = null!;
        public string? DiseaseName { get; set; }
        public AnalyticResultDto? AnalyticResult { get; set; }
        public MonitoringLogStatus Status { get; set; }
        public DateOnly? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateOnly? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsNewest { get; set; }
        public List<LogDetailDto> LogDetails { get; set; } = new();
        public List<ImageDto> Images { get; set; } = new();
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.MonitoringLogs, MonitoringLogDetailDto>()
                .ForMember(dest => dest.SampleName,
                    opt => opt.MapFrom(
                        src => src.SampleStage.Samples.Name))
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(
                        src => DateOnly.FromDateTime(src.CreatedDate)))
                .ForMember(dest => dest.SampleStageDefinitionName,
                    opt => opt.MapFrom(
                        src => src.SampleStage.SampleStageDefinition.Name))
                .ForMember(dest => dest.UpdatedDate,
                    opt => opt.MapFrom(src => src.UpdatedDate.HasValue
                        ? DateOnly.FromDateTime(src.UpdatedDate.Value)
                        : (DateOnly?)null))
                .ForMember(dest => dest.Images, opt => opt.Ignore());
        }
    }
}
