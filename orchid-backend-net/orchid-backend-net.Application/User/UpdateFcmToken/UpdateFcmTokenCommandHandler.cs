using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.UpdateFcmToken
{
    public class UpdateFcmTokenCommandHandler(IUserRepository userRepository)
        : IRequestHandler<UpdateFcmTokenCommand, string>
    {
        public async Task<string> Handle(UpdateFcmTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy user.");

            user.FcmToken = request.FcmToken;
            await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return "Cập nhật FCM token thành công.";
        }
    }
}