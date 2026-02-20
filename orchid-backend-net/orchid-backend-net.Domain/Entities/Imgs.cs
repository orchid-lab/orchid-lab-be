using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Imgs : BaseGuidEntity
    {
        public ImageTargetType TargetType { get; set; }
        public string TargetId { get; set; } = string.Empty;
        public required string Url { get; set; }
    }
}