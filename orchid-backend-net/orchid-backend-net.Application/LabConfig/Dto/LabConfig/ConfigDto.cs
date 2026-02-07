using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;

namespace orchid_backend_net.Application.LabConfig.Dto.LabConfig
{
    public class ConfigDto : IMapFrom<Domain.Entities.Config>
    {
        public string ConfigName { get; set; } = default!;
        public string Key { get; set; } = default!;
        public decimal Value { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            throw new NotImplementedException();
        }
    }
}
