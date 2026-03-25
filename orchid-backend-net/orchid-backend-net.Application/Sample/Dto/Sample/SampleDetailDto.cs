using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Sample.Dto.SampleStage;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Sample.Dto.Sample
{
    public class SampleDetailDto : IMapFrom<Samples>
    {
        public required string Id { get; set; }
        public string Name { get; set; } = default!;
        public required string ExperimentLogId { get; set; }
        public required string CurrentSampleStage { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateOnly? ExecutionDate { get; set; }
        public SampleStatus Status { get; set; } = default!;
        public List<SampleStageDto> SampleStageDto { get; set; } = default!;
        public string? InitialCondition { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Samples, SampleDetailDto>()
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.SampleStages
                    .Select(s => s.Status)
                    .FirstOrDefault()))
                 .ForMember(dest => dest.SampleStageDto,
                    opt => opt.MapFrom(src =>
                        src.SampleStages.OrderBy(ss => ss.SampleStageDefinitionId)))
                .ForMember(dest => dest.InitialCondition, opt => opt.MapFrom(src => src.InitialCondition))
                .ForMember(dest => dest.CurrentSampleStage,
                    opt => opt.MapFrom(src => src.SampleStages
                        .OrderByDescending(s => s.SampleStageDefinitionId)
                        .Select(ss => ss.SampleStageDefinition.Name)
                        .FirstOrDefault()));
        }
    }
}
