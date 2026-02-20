using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Images.Policy;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Images.UseCase.UploadImage
{
    public class UploadImageCommand(string fileName, byte[] fileStream, string targetType, string targetId) : IRequest<string>
    {
        public string FileName { get; set; } = fileName;
        public byte[] FileStream { get; set; } = fileStream;
        public string TargetType { get; set; } = targetType;
        public string TargetId { get; set; } = targetId;
    }

    internal class UploadImageCommandHandler(
        IImageUploaderService imageUploaderService,
        IImageRepository imageRepository,
        IMonitoringLogRepository monitoringLogRepository,
        ITaskRepository taskRepository) : IRequestHandler<UploadImageCommand, string>
    {
        public async Task<string> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            //validate target type 
            var parsedTargetType = await ImagePolicy.ValidateImageTargetType(request, monitoringLogRepository, taskRepository);
            //get folder base on target type
            var folder = GetFolderByTargetType(parsedTargetType);
            //upload image to storage and get url
            var imageUrl = await imageUploaderService.UpdloadImageAsync(request.FileStream, request.FileName, folder, cancellationToken);

            //add image into database
            var img = new Imgs
            {
                Url = imageUrl,
                TargetId = request.TargetId,
                TargetType = Enum.Parse<ImageTargetType>(request.TargetType)
            };

            imageRepository.Add(img);

            return await imageRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ?
                imageUrl : "Failed to save image info to database";
        }

        private static string GetFolderByTargetType(Domain.Common.Enum.ImageTargetType targetType)
        {
            return targetType switch
            {
                Domain.Common.Enum.ImageTargetType.MonitoringLog => "monitoring-logs",
                Domain.Common.Enum.ImageTargetType.Task => "tasks",
                _ => "general"
            };
        }

    }
}
