using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = folder ?? options.Value.DefaultFolder,
                UseFilename = options.Value.UseFilename,
                UniqueFilename = options.Value.UniqueFilename,
                Overwrite = true, 
                Invalidate = false,
                Transformation = new Transformation()
                .Width(512)
                .Height(512)
                .Crop("limit")
                .Quality(70)
                .FetchFormat("jpg")
            };

            var result = await cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(result.Error?.Message);

            return result.SecureUrl.ToString();
        }
    }
}
