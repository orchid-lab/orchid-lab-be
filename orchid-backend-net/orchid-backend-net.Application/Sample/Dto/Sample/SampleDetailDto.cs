using AutoMapper;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Sample.Dto.SampleStage;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Sample.Dto.Sample
{
    public class SampleDetailDto : IMapFrom<Samples>
    {
        public required string Id { get; set; }
        public string Name { get; set; } = default!;
        public required string ExperimentLogId { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public DateOnly? ExecutionDate { get; set; }
        public string Status { get; set; } = default!;
        public SampleStageDto SampleStageDto { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Samples, SampleDetailDto>()
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.SampleStages
                    .Select(s => s.Status.ToDisplayText()
                    )))
                .ForMember(dest => dest.SampleStageDto,
                    opt => opt.MapFrom(src => 
                        src.SampleStages
                            .Where(s => s.Status
                                .Equals(Domain.Common.Enum.SampleStatus.InProgressed)
                            )
                    )
                );
        }
    }
}
