using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Method.Dto.Method
{
    public class GetSuccessCompletedExperimentLogRateDto
        : IMapFrom<Domain.Entities.Methods>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int? TotalDurationDays { get; set; }
        public int? CompletedExperimentLog { get; set; }
        public int? FailedExperimentLog { get; set; }
        public int? SuccessRate { get; set; }
        public List<Seedlings>? Seedling { get; set; }
        public List<Domain.Entities.MethodStageDefinition>? MethodStages { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Methods, GetSuccessCompletedExperimentLogRateDto>()
               .ForMember(dest => dest.TotalDurationDays,
               opt => opt.MapFrom(
                   src => src.MethodStages.Sum(ms => ms.DurationsDays)))
               .ForMember(dest => dest.CompletedExperimentLog,
               opt => opt.MapFrom(
                   src => src.ExperimentLogs.Count(el => el.Status == ExperimentLogStatus.Completed && el.MethodId == this.Id)))
               .ForMember(dest => dest.FailedExperimentLog,
               opt => opt.MapFrom(
                   src => src.ExperimentLogs.Count(el => el.Status == ExperimentLogStatus.Cancelled && el.MethodId == this.Id)))
               .ForMember(dest => dest.MethodStages,
               opt => opt.MapFrom(
                   src => src.ExperimentLogs.Where(el => el.MethodId.Equals(this.Id) && el.Status.Equals(ExperimentLogStatus.Cancelled))));
        }
    }
}
