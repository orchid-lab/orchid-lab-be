using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Images;
using orchid_backend_net.Application.ReportAttribute;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Enums;

namespace orchid_backend_net.Application.Report
{
    public class ReportDTO : IMapFrom<Reports>
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sample { get; set; }
        public string Technician { get; set; }
        public ReportStatus Status { get; set; }
        public string? ReviewReport {  get; set; }
        public List<ReportAttributesDTO> ReportAttributes { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Reports, ReportDTO>()
                .ForMember(dest => dest.Sample, opt => opt.MapFrom(src => src.SampleID))
                .ForMember(dest => dest.Technician, opt => opt.MapFrom(src => src.Technician.Name))
                .ForMember(dest => dest.ReportAttributes, opt => opt.MapFrom(src => src.ReportAttributes))
                .ForMember(dest => dest.Status, otp => otp.MapFrom(src => (ReportStatus)src.Status))
                .ForMember(dest => dest.Status, otp => otp.MapFrom(src => (int)src.Status))
                .ReverseMap();
        }
    }
}
