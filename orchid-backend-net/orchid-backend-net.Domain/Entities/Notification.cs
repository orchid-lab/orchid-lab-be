using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Notification : BaseGuidEntity
    {
        public string UserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public NotificationTargetType NotificationTargetType { get; set; }
        public string? TargetId { get; set; }
    }
}
