using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Referent
{
    public class ReferentDTO : IMapFrom<Referents>
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public decimal ValueFrom { get; set; }
        public decimal ValueTo { get; set; }
        public string MeasurementUnit { get; set; }
        public bool Status { get; set; }
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Referents, ReferentDTO>()
                .ReverseMap();
        }
    }
}
