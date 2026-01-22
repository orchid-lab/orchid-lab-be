using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Notification.Dto;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Notification.UseCase.GetAllNotification
{
    public record GetAllNotificationQuery(
        int PageNo, 
        int PageSize, 
        string UserId) : IRequest<PageResult<NotificationDto>>;
    internal class GetAllNotificationQueryHandler(
        INotificationRepository notificationService) : IRequestHandler<GetAllNotificationQuery, PageResult<NotificationDto>>
    {
        public async Task<PageResult<NotificationDto>> Handle(GetAllNotificationQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.Notification> queryOptions(IQueryable<Domain.Entities.Notification> query)
            {
               return query.Where(n => n.UserId == request.UserId);
            }
            var pageResult = await notificationService.FindAllProjectToAsync<NotificationDto>(
                request.PageNo,
                request.PageSize,
                queryOptions,
                cancellationToken: cancellationToken);
            return pageResult.ToAppPageResult();
        }
    }
}
