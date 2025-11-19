using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Images;
using orchid_backend_net.Application.Images.Create;
using orchid_backend_net.Application.Images.GetAll;
using orchid_backend_net.Application.Images.GetInfor;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers.Images
{
    [Route("api/images")]
    [ApiController]
    public class ImgsController(ISender sender, ILogger<ImgsController> logger) : BaseController(sender)
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<ImagesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<PageResult<ImagesDTO>>>> GetAllMethods(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] string? reportId,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(new GetAllImagesQuery(pageNumber, pageSize, reportId), cancellationToken);
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<PageResult<ImagesDTO>>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<ImagesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<ImagesDTO>>> GetAllMethods(
          [FromRoute] string id,
           CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(new GetImagesInforQuery(id), cancellationToken);
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<ImagesDTO>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Create(
            [FromForm] List<IFormFile> images,
            [FromForm] string reportId,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                if (string.IsNullOrEmpty(reportId))
                    return BadRequest("Thiếu report Id.");
                if (images.Count == 0)
                {
                    return BadRequest("Không có hình nào được gửi.");
                }
                foreach (var file in images)
                {
                    using var stream = file.OpenReadStream();
                    stream.Position = 0;
                    var command = new CreateImageCommand(stream, file.FileName, reportId);
                    var imageResult = await sender.Send(command, cancellationToken);
                    if (imageResult.Equals(false))
                    {
                        return BadRequest("Lỗi khi tải hình ảnh: " + file.FileName);
                    }
                }
                return Ok(new JsonResponse<string>("Tải hình ảnh thành công."));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi tải hình ảnh với lỗi sau: " + ex.Message);
            }
        }
    }
}
