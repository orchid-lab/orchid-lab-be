using AutoMapper;
using orchid_backend_net.Application.Batch.Dto.Batch;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Method.Dto.Method;
using orchid_backend_net.Application.Sample.Dto;
using orchid_backend_net.Application.Seedling.Dto;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.ExperimentLog.Dto.ExperimentLog
{
    public class ExperimentLogDetailDto : IMapFrom<ExperimentLogs>
    {
        public string Id { get; set; }
        public required SeedlingsDetailDto Seedling { get; set; }
        public required MethodDetailDto Method { get; set; }
        public required BatchDto Batch { get; set; }
        public int ExpectedSampleCount { get; set; }
        public int CurrentStageOrder { get; set; }
        public required string Name { get; set; }
        public required string AssignedTo { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public ExperimentLogStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public List<SampleDto>? Samples { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExperimentLogs, ExperimentLogDetailDto>()
                .ForMember(dest => dest.Seedling,
                opt => opt.MapFrom(src => src.SeedlingParent))
                .ForMember(dest => dest.Method,
                opt => opt.MapFrom(src => src.Method))
                .ForMember(dest => dest.Batch,
                opt => opt.MapFrom(src => src.Batch))
               .ForMember(dest => dest.Samples,
                opt => opt.MapFrom(src => src.Samples));
        }
    }
}
