using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.MonitoringLog.Dto.LogDetailDto;

namespace orchid_backend_net.Application.Sample.Dto.SampleStage
{
    public class SampleStageDto : IMapFrom<Domain.Entities.SampleStage>
    {
        public required string Id { get; set; }
        public DateOnly StartAt { get; set; }
        public required string CurrentSampleStage { get; set; }
        public List<LogDetailDtos> LogDetailDtos { get; set; } = new();
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.SampleStage, SampleStageDto>()
                .ForMember(dest => dest.CurrentSampleStage,
                    opt => opt.MapFrom(
                        src => src.SampleStageDefinition.Name))
                .ForMember(dest => dest.LogDetailDtos,
                    opt => opt.MapFrom(
                        src => src.MonitoringLogs
                            .Where(m => m.IsNewest)
                            .Select(m => m.LogDetails)
                    )
                );
        }
    }
}
