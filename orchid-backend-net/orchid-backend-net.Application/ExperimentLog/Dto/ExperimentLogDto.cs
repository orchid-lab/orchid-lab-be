using AutoMapper;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Sample.Dto;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.ExperimentLog.Dto
{
    internal class ExperimentLogDto : IMapFrom<ExperimentLogs>
    {
        public string HybridzationId { get; set; } = default!;
        public int MethodId { get; set; }
        public int BatchId { get; set; }
        public string MethodName { get; set; } = default!;
        public required string Name { get; set; }
        public required string CreatedBy { get; set; }
        public required string AssignedTo { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; }
        public List<SampleDto> Samples { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExperimentLogs, ExperimentLogDto>()
                .ForMember(dest => dest.MethodName, 
                opt => opt.MapFrom(src => src.Method.Name))
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToDisplayText()));
        }
    }
}
