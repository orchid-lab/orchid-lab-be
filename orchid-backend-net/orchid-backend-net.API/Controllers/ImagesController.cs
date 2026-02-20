using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Images.UseCase.UploadImage;
using orchid_backend_net.Application.Images.UseCase.UploadUserAvatarCommand;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Infrastructure.Service;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// using for cloudinary service and images table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/images")]
    [ApiController]
    public class ImagesController(ISender sender, ILogger<ImagesController> logger) : BaseController(sender)
    {
        /// <summary>
        /// using for upload user image in return of the url
        /// </summary>
        /// <param name="image"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/user")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 201)]
        public async Task<ActionResult<JsonResponse<string>>> UploadUserImage(IFormFile image, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                if (image == null || image.Length == 0)
                    return BadRequest("Image file is required.");

                byte[] originalBytes;
                await using (var ms = new MemoryStream((int)image.Length))
                {
                    await image.CopyToAsync(ms, cancellationToken);
                    originalBytes = ms.ToArray();
                }

                var resizedBytes = ResizeAndCompressingImage
                    .ResizeAndCompressImages([.. originalBytes], 512, 512, 70);

                var command = new UploadUserAvatarCommand(image.FileName, resizedBytes);
                var result = await Sender.Send(command, cancellationToken);

                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Người dùng cập nhật hình thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// upload image based on the targetType and targetId, for example: targetType = "MonitoringLog", targetId = "123" => image will be uploaded for MonitoringLog with id 123
        /// <remarks>
        /// Target type has only 2 type
        /// <ul>
        /// <li><c>MonitoringLog</c></li>
        /// <li><c>Task</c></li>
        /// </ul>
        /// </remarks>
        /// </summary>
        /// <param name="image"></param>
        /// <param name="targetType"></param>
        /// <param name="targetId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult<JsonResponse<string>>> UploadImage(
            [FromForm] IFormFile image,
            [FromForm] string targetType, 
            [FromForm] string targetId, 
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Uploading image for {TargetType}:{TargetId}", targetType, targetId);

                if (image == null || image.Length == 0)
                    return BadRequest("Image file is required.");

                if (string.IsNullOrWhiteSpace(targetType))
                    return BadRequest("TargetType is required.");

                if (string.IsNullOrWhiteSpace(targetId))
                    return BadRequest("TargetId is required.");

                // Convert IFormFile to byte array
                byte[] originalBytes;
                await using (var ms = new MemoryStream((int)image.Length))
                {
                    await image.CopyToAsync(ms, cancellationToken);
                    originalBytes = ms.ToArray();
                }

                // Resize and compress
                var resizedBytes = ResizeAndCompressingImage
                    .ResizeAndCompressImages([.. originalBytes], 512, 512, 70);

                // Create command
                var command = new UploadImageCommand(
                    image.FileName,
                    resizedBytes,
                    targetType,    // Pass as string
                    targetId
                );

                var imageUrl = await Sender.Send(command, cancellationToken);

                return Ok(new JsonResponse<string>(imageUrl));
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "Validation error at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails
                {
                    Title = "Dữ liệu không hợp lệ",
                    Detail = ex.Message
                });
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Not found at {Time}", DateTime.UtcNow);
                return NotFound(new ProblemDetails
                {
                    Title = "Không tìm thấy",
                    Detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error uploading image at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails
                {
                    Title = "Upload thất bại",
                    Detail = ex.Message
                });
            }
        }
    }
}
