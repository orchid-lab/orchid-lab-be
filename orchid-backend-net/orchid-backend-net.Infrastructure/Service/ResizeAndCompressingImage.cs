using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace orchid_backend_net.Infrastructure.Service
{
    internal static class ResizeAndCompressingImage
    {
        public static byte[] ResizeAndCompressImages(byte[] imageBytes, int maxWidth, int maxHeight, int quality = 70)
        {
            using var image = Image.Load(imageBytes);
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            image.Mutate(x => x.Resize(newWidth, newHeight));

            var encoder = new JpegEncoder { Quality = quality };
            using var outputStream = new MemoryStream();
            image.Save(outputStream, encoder);
            return outputStream.ToArray();
        }
    }
}