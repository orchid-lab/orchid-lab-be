using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Infrastructure.Service.CloudinarySettings;
using System.Diagnostics;
using System.Net;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class CloudinaryImageUploaderService(Cloudinary cloudinary, IOptions<CloudinaryOptions> options) : IImageUploaderService
    {
        public async Task<string> UpdloadImageAsync(byte[] imageBytes, string fileName, string? folder = null, CancellationToken cancellationToken = default)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                throw new ArgumentException("Image bytes cannot be empty");

            cancellationToken.ThrowIfCancellationRequested();

            var sw = new Stopwatch();
            sw.Start();

            await using var stream = new MemoryStream(imageBytes);

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
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
                    .FetchFormat("jpg"),
            };

            var result = await cloudinary.UploadAsync(uploadParams);

            sw.Stop();

            if (result.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(result.Error?.Message);

            return result.SecureUrl.ToString();
        }
    }
}
