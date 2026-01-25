using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Sample.Dto.StageRequirementDefinitionDto;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.MonitoringLog.Dto.LogDetailDto
{
    public class LogDetailDtos : IMapFrom<LogDetails>
    {
        public required string Id { get; set; }
        public required decimal MeasuredValue { get; set; }
        public required bool IsMatch { get; set; }
        public StageRequirementDefinitionDto StageRequirementDefinitionDto { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<LogDetails, LogDetailDtos>();
        }
    }
}
