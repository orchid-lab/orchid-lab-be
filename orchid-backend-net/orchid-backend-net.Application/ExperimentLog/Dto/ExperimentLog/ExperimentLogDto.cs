using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.ExperimentLog.Dto.ExperimentLog
{
    public class ExperimentLogDto : IMapFrom<ExperimentLogs>
    {
        public required string Id { get; set; }
        public string Name { get; set; } = default!;
        public int CurrentStageOrder { get; set; }
        public string MethodName { get; set; } = default!;
        public string BatcheName { get; set; } = default!;
        public int ExpectedSampleCount { get; set; }
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedDate { get; set; }
        public ExperimentLogStatus Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExperimentLogs, ExperimentLogDto>()
                .ForMember(dest => dest.MethodName,
                opt => opt.MapFrom(src => src.Method.Name))
                .ForMember(dest => dest.BatcheName,
                opt => opt.MapFrom(src => src.Batch.BatchName))
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status));
        }
    }
}
