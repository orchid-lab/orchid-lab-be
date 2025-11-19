using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;
using System.IO;

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
            try
            {
                var imageUrl = await imageUploaderService.UpdloadImageAsync(request.FileStream, request.FileName, "user-avatar");
                var user = await userRepository.FindAsync(x => x.ID.Equals(request.Id) && x.Status, cancellationToken);
                user.AvatarUrl = imageUrl;
                user.Update_date = DateTime.UtcNow;
                user.Update_by = currentUserService.UserName;
                userRepository.Update(user);
                return await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                    ? "Update user avatar successfully"
                    : "Update user avatar failed";
            }
            catch(Exception ex)
            {
                throw new Exception("Error while updating user avatar", ex);
            }
        }
    }
}
