using MediatR;
using orchid_backend_net.Application.Common.Interfaces;

namespace orchid_backend_net.Application.Images.UseCase.UploadUserAvatarCommand
{
    public class UploadUserAvatarCommand(string fileName, byte[] fileStream) : IRequest<string>
    {
        public string FileName { get; set; } = fileName;
        public byte[] FileStream { get; set; } = fileStream;
    }

    internal class UploadUserAvatarCommandHandler(IImageUploaderService imageUploaderService) : IRequestHandler<UploadUserAvatarCommand, string>
    {
        public async Task<string> Handle(UploadUserAvatarCommand request, CancellationToken cancellationToken)
        {
            var imageUrl = await imageUploaderService.UpdloadImageAsync(request.FileStream, request.FileName, "user-avatar");
            return imageUrl;
        }
    }
}
