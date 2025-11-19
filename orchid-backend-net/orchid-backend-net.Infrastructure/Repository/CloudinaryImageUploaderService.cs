using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Infrastructure.Service.CloudinarySettings;
using System.Net;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class CloudinaryImageUploaderService(Cloudinary cloudinary, IOptions<CloudinaryOptions> options) : IImageUploaderService
    {
        public async Task<string> UpdloadImageAsync(Stream fileStream, string fileName, string? folder = null)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                throw new ArgumentException("File stream cannot be null or empty.", nameof(fileStream));
            }

            await using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0; // Reset the stream position to the beginning

            var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams
            {
                File = new FileDescription(fileName, memoryStream),
                Folder = folder ?? options.Value.DefaultFolder,
                UseFilename = options.Value.UseFilename,
                UniqueFilename = options.Value.UniqueFilename,
                Overwrite = options.Value.Overwrite
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Image upload failed: {uploadResult.Error?.Message}");
            }
                
            return uploadResult.SecureUrl.ToString();
        }
    }
}
