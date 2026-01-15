using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Images.UseCase.UploadUserAvatarCommand;
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
        [HttpPost]
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
                return BadRequest(new ProblemDetails { Title = "User update failed", Detail = ex.Message });
            }
        }
    }
}
