using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.MonitoringLog.Dto.AnalyticResult
{
    public class AnalyticResultDto : IMapFrom<Domain.Entities.AnalyticResults>
    {
        public required string Id { get; set; }
        public required decimal Anthracnose { get; set; }
        public required decimal BacterialWilt { get; set; }
        public required decimal Blackrot { get; set; }
        public required decimal Brownspots { get; set; }
        public required decimal MoldBacterial { get; set; }
        public required decimal MoldFungus { get; set; }
        public required decimal SoftRot { get; set; }
        public required decimal StemRot { get; set; }
        public required decimal WitheredYellowRoot { get; set; }
        public required decimal Healthy { get; set; }
        public required decimal Oxidation { get; set; }
        public required decimal Virus { get; set; }

        public static AnalyticResultDto Create(AnalyticResults entity)
        {
            return new AnalyticResultDto
            {
                Id = entity.ID.ToString(),
                Anthracnose = entity.Anthracnose,
                BacterialWilt = entity.BacterialWilt,
                Blackrot = entity.Blackrot,
                Brownspots = entity.Brownspots,
                MoldBacterial = entity.MoldBacterial,
                MoldFungus = entity.MoldFungus,
                SoftRot = entity.SoftRot,
                StemRot = entity.StemRot,
                WitheredYellowRoot = entity.WitheredYellowRoot,
                Healthy = entity.Healthy,
                Oxidation = entity.Oxidation,
                Virus = entity.Virus
            };
        }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.AnalyticResults, AnalyticResultDto>();
        }
    }
}
