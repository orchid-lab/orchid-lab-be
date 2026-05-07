using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Common.Enum;

namespace orchid_backend_net.Application.Notification.Dto
{
    public class NotificationDto : IMapFrom<Domain.Entities.Notification>
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public NotificationTargetType NotificationTargetType { get; set; }
        public string? TargetId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Notification, NotificationDto>();
        }
    }
}
