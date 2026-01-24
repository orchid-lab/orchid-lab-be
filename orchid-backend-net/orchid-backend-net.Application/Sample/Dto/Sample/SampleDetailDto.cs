using AutoMapper;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Sample.Dto.Sample
{
    public class SampleDetailDto : IMapFrom<Samples>
    {
        public string Name { get; set; } = default!;
        public required string ExperimentLogId { get; set; } 
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public DateOnly? ExecutionDate { get; set; }
        public string Status { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Samples, SampleDetailDto>()
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.SampleStages
                .Select(
                    s => 
                    s.Status.ToDisplayText()
                    )));
        }
    }
}
