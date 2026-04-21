using orchid_backend_net.Application.Images.UseCase.UploadImage;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Images.Policy
{
    public static class ImagePolicy
    {
        private static readonly HashSet<ImageTargetType> AllowedImageTargetTypes =
        [
            ImageTargetType.Task,
            ImageTargetType.MonitoringLog,
            ImageTargetType.Sample,
            ImageTargetType.SampleStage
        ];

        public static async Task<ImageTargetType> ValidateImageTargetType(
            UploadImageCommand request,
            IMonitoringLogRepository monitoringLogRepository,
            ITaskRepository taskRepository,
            ISampleRepository sampleRepository)
        {
            if(!Enum.TryParse<ImageTargetType>(request.TargetType, true, out var parsedTargetType))
            {
                throw new ArgumentException($"Target type cho image không hợp lệ: {request.TargetType}");
            }

            if (!AllowedImageTargetTypes.Contains(parsedTargetType))
            {
                throw new ArgumentException($"Target type cho image không được phép: {request.TargetType}");
            }

            var exist = parsedTargetType switch
            {
                ImageTargetType.MonitoringLog => await monitoringLogRepository.AnyAsync(m => m.ID.Equals(request.TargetId), CancellationToken.None),
                ImageTargetType.Task => await taskRepository.AnyAsync(t => t.ID.Equals(request.TargetId), CancellationToken.None),
                ImageTargetType.Sample => await sampleRepository.AnyAsync(s => s.ID.Equals(request.TargetId), CancellationToken.None),  
                ImageTargetType.SampleStage => await sampleRepository.AnyAsync(s => s.SampleStages.Any(ss => ss.ID.Equals(request.TargetId)), CancellationToken.None),
                _ => false
            };

            if(!exist) 
                throw new NotFoundException("Target ID không tồn tại: {request.TargetId}");

            return parsedTargetType;
        }
    }
}
