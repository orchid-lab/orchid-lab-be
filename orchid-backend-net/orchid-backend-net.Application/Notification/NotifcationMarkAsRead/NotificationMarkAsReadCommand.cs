using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Notification.NotifcationMarkAsRead
{
    public class NotificationMarkAsReadCommand : IRequest<string>
    {
        public required string NotifcationId { get; set; }

        public NotificationMarkAsReadCommand() { }
        public NotificationMarkAsReadCommand(string notifcationId)
        {
            NotifcationId = notifcationId;
        }
    }

    internal class NotificationMarkAsReadCommandHandler(INotificationRepository notificationRepository) : IRequestHandler<NotificationMarkAsReadCommand, string>
    {
        public async Task<string> Handle(NotificationMarkAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await notificationRepository.FindAsync(n => n.ID == request.NotifcationId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy thông báo này");

            notification.IsRead = true;

            notificationRepository.Update(notification);
            return await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? "Cập nhật thành công." : "Cập nhật thất bại.";
        }
    }
}
