using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Notification.Helper
{
    public static class CreateNotificationHelper
    {
        public static List<Domain.Entities.Notification> CreateForMultipleUsers(
            IEnumerable<Users> users,
            string title,
            string content)
            => users.Select(user => CreateForSingleUsers(user.ID, title, content)).ToList();

        public static Domain.Entities.Notification CreateForSingleUsers(
            string userId,
            string title,
            string content)
        {
            var now = DateTime.UtcNow;
            return new Domain.Entities.Notification
            {
                Title = title,
                Content = content,
                UserId = userId,
                IsRead = false,
                CreatedAt = now
            };
        }
    }
}
