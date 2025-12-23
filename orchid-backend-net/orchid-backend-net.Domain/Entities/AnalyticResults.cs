using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class AnalyticResults : BaseGuidEntity
    {
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

        public virtual MonitoringLogs MonitoringLog { get; set; }
    }
}