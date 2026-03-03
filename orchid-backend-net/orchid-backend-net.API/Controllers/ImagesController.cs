using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.Image;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Images.Dto.Img;
using orchid_backend_net.Application.Images.UseCase.GetAll;
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
        /// return a list with pagination of image 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<ImageDto>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllImageQuery query, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// using for upload user image in return of the url
        /// </summary>
        /// <param name="image"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("user")]
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
        /// Upload image with polymorphic association.
        /// </summary>
        /// <param name="request">Upload request containing image, target type and target ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Image URL from Cloudinary</returns>
        /// <remarks>
        /// Target type allowed values:
        /// <ul>
        /// <li><c>MonitoringLog</c> or <c>0</c></li>
        /// <li><c>Task</c> or <c>1</c></li>
        /// </ul>
        /// 
        /// Sample request:
        /// 
        ///     POST /api/images
        ///     Content-Type: multipart/form-data
        ///     
        ///     image: [binary file]
        ///     targetType: MonitoringLog
        ///     targetId: 123e4567-e89b-12d3-a456-426614174000
        /// 
        /// </remarks>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<ActionResult<JsonResponse<string>>> UploadImage(
            [FromForm] UploadImageRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Uploading image for {TargetType}:{TargetId}",
                    request.TargetType, request.TargetId);

                if (request.Image == null || request.Image.Length == 0)
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Dữ liệu không hợp lệ",
                        Detail = "Image file is required."
                    });

                if (string.IsNullOrWhiteSpace(request.TargetType))
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Dữ liệu không hợp lệ",
                        Detail = "TargetType is required."
                    });

                if (string.IsNullOrWhiteSpace(request.TargetId))
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Dữ liệu không hợp lệ",
                        Detail = "TargetId is required."
                    });

                // Convert IFormFile to byte array
                byte[] originalBytes;
                await using (var ms = new MemoryStream((int)request.Image.Length))
                {
                    await request.Image.CopyToAsync(ms, cancellationToken);
                    originalBytes = ms.ToArray();
                }

                // Resize and compress
                var resizedBytes = ResizeAndCompressingImage
                    .ResizeAndCompressImages([.. originalBytes], 512, 512, 70);

                // Create command
                var command = new UploadImageCommand(
                    request.Image.FileName,
                    resizedBytes,
                    request.TargetType,
                    request.TargetId
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
