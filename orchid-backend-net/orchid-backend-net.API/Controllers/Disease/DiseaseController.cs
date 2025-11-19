using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Disease;
using orchid_backend_net.Application.Disease.Analysis;
using orchid_backend_net.Application.Disease.Create;
using orchid_backend_net.Application.Disease.Delete;
using orchid_backend_net.Application.Disease.GetAll;
using orchid_backend_net.Application.Disease.GetInfor;
using orchid_backend_net.Application.Disease.Update;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers.Disease
{
    [Route("api/disease")]
    [ApiController]
    public class DiseaseController(ISender sender, ILogger<DiseaseController> logger) : BaseController(sender)
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<DiseaseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<DiseaseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<PageResult<DiseaseDTO>>>> GetAll(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] string? searchTerm,
            [FromQuery] decimal? minInfectedRate,
            [FromQuery] bool? isActive,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await sender.Send(new GetAllDiseaseQuery(pageNumber, pageSize, searchTerm, minInfectedRate, isActive), cancellationToken);
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<PageResult<DiseaseDTO>>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<DiseaseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<DiseaseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<DiseaseDTO>>> GetInfor(
           [FromRoute] string id,
           CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await sender.Send(new GetDiseaseInforQuery(id), cancellationToken);
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<DiseaseDTO>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Create(
           [FromBody] CreateDiseaseCommand command,
           CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await sender.Send(command, cancellationToken);
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi tạo với lỗi sau: " + ex.Message);
            }
        }

        [HttpPost("analyze")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<AnalysisDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<AnalysisDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<AnalysisDTO>>> AnalyzeImage(
           IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                    return BadRequest("Image file is required.");

                using var stream = imageFile.OpenReadStream();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                var command = new DiseaseAnalysisCommand { ImageBytes = imageBytes };
                var result = await _sender.Send(command);
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi phân tích với lỗi sau: " + ex.Message);
            }
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Update(
           [FromBody] UpdateDiseaseCommand command,
           CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await sender.Send(command, cancellationToken);
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi cập nhật với lỗi sau: " + ex.Message);
            }
        }

        [HttpDelete]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Delete(
           [FromBody] DeleteDiseaseCommand command,
           CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await sender.Send(command, cancellationToken);
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing DELETE request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi xóa với lỗi sau: " + ex.Message);
            }
        }
    }
}
