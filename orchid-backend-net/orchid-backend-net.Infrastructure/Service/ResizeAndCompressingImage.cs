using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace orchid_backend_net.Infrastructure.Service
{
    public static class ResizeAndCompressingImage
    {
        /// <summary>
        /// Resize và compress cho display/upload (giữ tỉ lệ, JPEG compression).
        /// Dùng cho: Avatar, gallery images, monitoring log images.
        /// </summary>
        public static byte[] ResizeAndCompressImages(byte[] imageBytes, int maxWidth, int maxHeight, int quality = 70)
        {
            using var inputStream = new MemoryStream(imageBytes, writable: false);
            return ResizeAndCompressImages(inputStream, maxWidth, maxHeight, quality);
        }

        /// <summary>
        /// Resize và compress cho display/upload (giữ tỉ lệ, JPEG compression).
        /// </summary>
        public static byte[] ResizeAndCompressImages(Stream inputStream, int maxWidth, int maxHeight, int quality = 70)
        {
            using var image = Image.Load(inputStream);
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(1.0, Math.Min(ratioX, ratioY));

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            image.Mutate(x => x.Resize(newWidth, newHeight));

            var encoder = new JpegEncoder { Quality = quality };
            using var outputStream = new MemoryStream();
            image.Save(outputStream, encoder);
            return outputStream.ToArray();
        }

        /// <summary>
        /// Validate và prepare image cho AI inference (không resize, không compress).
        /// <ul>
        /// <li>Validates image format and dimensions</li>
        /// <li>Returns lossless PNG format</li>
        /// <li>Actual resize will be performed by OnnxOrchidAnalyzerService based on model input shape</li>
        /// </ul>
        /// </summary>
        /// <param name="inputStream">Input image stream</param>
        /// <returns>PNG-encoded image bytes (lossless)</returns>
        /// <exception cref="ArgumentException">If image dimensions are invalid</exception>
        public static byte[] PrepareForInference(Stream inputStream)
        {
            using var image = Image.Load(inputStream);
            
            // Validate minimum size
            if (image.Width < 50 || image.Height < 50)
                throw new ArgumentException("Image too small for analysis (minimum 50x50 pixels required)");

            // Validate maximum size để tránh OOM trên VPS 4GB
            if (image.Width > 4000 || image.Height > 4000)
                throw new ArgumentException("Image too large for analysis (maximum 4000x4000 pixels)");

            // Return as PNG (lossless) - không resize, không compress
            using var outputStream = new MemoryStream();
            image.SaveAsPng(outputStream);
            return outputStream.ToArray();
        }

        /// <summary>
        /// Validate và prepare image cho AI inference (overload for byte[]).
        /// </summary>
        public static byte[] PrepareForInference(byte[] imageBytes)
        {
            using var inputStream = new MemoryStream(imageBytes, writable: false);
            return PrepareForInference(inputStream);
        }
    }
}