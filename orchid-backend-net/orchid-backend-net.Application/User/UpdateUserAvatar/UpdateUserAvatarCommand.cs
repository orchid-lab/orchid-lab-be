using MediatR;
using orchid_backend_net.Application.Common.Helper;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.UpdateUserAvatar
{
    public class UpdateUserAvatarCommand(string id, string fileName, Stream fileStream) : IRequest<string>, ICommand
    {
        public string Id { get; set; } = id;
        public string FileName { get; set; } = fileName;
        public Stream FileStream { get; set; } = fileStream;
    }

    internal class UpdateUserAvatarCommandHandler(IUserRepository userRepository, ICurrentUserService currentUserService
        , IImageUploaderService imageUploaderService) : IRequestHandler<UpdateUserAvatarCommand, string>
    {
        public async Task<string> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
        {
            var imageUrl = await imageUploaderService.UpdloadImageAsync(request.FileStream, request.FileName, "user-avatar");
            var user = await userRepository.FindAsync(x => x.ID.Equals(request.Id) && x.DeletedDate == null, cancellationToken);
            if (user == null)
                throw new NotFoundException("Người dùng không hợp lệ.");
            user.AvatarUrl = imageUrl;
            user.UpdatedDate = TimeZoneHelper.VietnamTimeNow;
            user.UpdatedBy = currentUserService.UserName;
            userRepository.Update(user);
            return await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Đổi hình đại diện thành công."
                : "Đổi hình đại diện thất bại.";
        }
    }
}
