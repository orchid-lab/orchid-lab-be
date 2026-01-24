using AutoMapper;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Sample.Dto.Sample
{
    public class SampleDto : IMapFrom<Samples>
    {
        public string Name { get; set; } = null!;
        public required string ExperimentLogId { get; set; }
        public string? CurrentSampleStage { get;set;  }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public DateOnly? ExecutionDate { get; set; }
        public string Status { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Samples, SampleDto>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src =>
                        src.SampleStages
                            .Where(s => s.Status == Domain.Common.Enum.SampleStatus.InProgressed)
                            .Select(s => s.Status.ToDisplayText())
                            .FirstOrDefault()
                    ))
                .ForMember(dest => dest.CurrentSampleStage,
                    opt => opt.MapFrom(src =>
                        src.SampleStages
                            .Where(s => s.Status == Domain.Common.Enum.SampleStatus.InProgressed)
                            .Select(s => s.SampleStageDefinition.Name)
                            .FirstOrDefault()
                    ));
        }
    }
}
