using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Method.Dto.MethodStage;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Method.Dto.Method
{
    public class MethodDetailDto : IMapFrom<Methods>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int TotalDurationDays { get; set; }
        public required List<MethodStageDetailDto> MethodStages { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Methods, MethodDetailDto>()
                .ForMember(dest => dest.TotalDurationDays, 
                opt => opt.MapFrom(
                    src => src.MethodStages.Sum(ms => ms.DurationsDays)))
                .ForMember(dest => dest.MethodStages, 
                opt => opt.MapFrom(
                    src => src.MethodStages));
        }
    }
}
