using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Stage;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Enums;

namespace orchid_backend_net.Application.ExperimentLog
{
    public class ExperimentLogDTO : IMapFrom<ExperimentLogs>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MethodName { get; set; }
        public string Description { get; set; }
        public string TissueCultureBatchID { get; set; }
        public string TissueCultureBatchName { get; set; }
        public string CurrentStageName { get; set; }
        public List<StageDTO> Stages { get; set; }
        public List<HybridzationDTO> Hybridizations { get; set; }
        public ExperimentLogStatus Status { get; set; }
        public string? Create_by { get; set; }
        public DateTime? Create_date { get; set; }
        public string? Update_by { get; set; }
        public DateTime? Update_date { get; set; }
        public string? Delete_by { get; set; }
        public DateTime? Delete_date { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExperimentLogs, ExperimentLogDTO>()
                .ForMember(dest => dest.TissueCultureBatchName, opt => opt.MapFrom(src => src.TissueCultureBatch.Name))
                .ForMember(dest => dest.MethodName, opt => opt.MapFrom(src => src.Method.Name))
                .ForMember(dest => dest.Stages, otp => otp.MapFrom(src => src.Method.Stages))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (ExperimentLogStatus)src.Status))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(dest => dest.CurrentStageName, opt => opt.MapFrom(src => src.CurrentStageID != null
                    ? src.Method.Stages.Where(s => s.ID == src.CurrentStageID).Select(s => s.Name).FirstOrDefault()
                    : null))
                .ForMember(dest => dest.Hybridizations, opt => opt.MapFrom(src => src.Hybridizations.Select(h => HybridzationDTO.Create(new GetSeedlingsNameDTO
                {
                    Id = h.Parent.ID,
                    LocalName = h.Parent.LocalName,
                    ScientificName = h.Parent.ScientificName
                }))))
                .ReverseMap();
        }
    }
}
