using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.ReportAttribute
{
    public class ReportAttributesDTO : IMapFrom<ReportAttributes>
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int Status { get; set; }
        public decimal ValueFrom { get; set; }
        public decimal ValueTo { get; set; }
        public string MeasurementUnit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReportAttributes, ReportAttributesDTO>()
                .ForMember(dest => dest.ValueFrom, opt => opt.MapFrom(src => src.Referent.ValueFrom))
                .ForMember(dest => dest.ValueTo, opt => opt.MapFrom(src => src.Referent.ValueTo))
                .ForMember(dest => dest.MeasurementUnit, opt => opt.MapFrom(src => src.Referent.MeasurementUnit))
                .ReverseMap();
        }
    }
}
