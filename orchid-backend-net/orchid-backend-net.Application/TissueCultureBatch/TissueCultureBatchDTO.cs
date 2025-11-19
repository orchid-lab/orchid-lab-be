using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.TissueCultureBatch
{
    public class TissueCultureBatchDTO : IMapFrom<TissueCultureBatches>
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string LabName { get; set; }
        public string LabRoomID { get; set; }
        public string Description { get; set; }
        public string InUse { get; set; }
        public bool Status { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<TissueCultureBatches, TissueCultureBatchDTO>()
                .ForMember(dest => dest.LabName, opt => opt.MapFrom(src => src.LabRoom.Name))
                .ForMember(dest => dest.InUse, opt 
                => opt.MapFrom(src => src.ExperimentLogs.FirstOrDefault(eL => eL.Status == 0).Name))
                .ReverseMap();
        }
    }
}
