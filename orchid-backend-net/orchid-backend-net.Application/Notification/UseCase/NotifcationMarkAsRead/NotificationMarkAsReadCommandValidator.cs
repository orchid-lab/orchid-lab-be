using FluentValidation;

namespace orchid_backend_net.Application.Notification.UseCase.NotifcationMarkAsRead
{
    public class NotificationMarkAsReadCommandValidator : AbstractValidator<NotificationMarkAsReadCommand>
    {
        public NotificationMarkAsReadCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty().WithMessage("Id không được để trống.");
        }
    }
}
