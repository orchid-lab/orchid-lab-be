using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Seedling;
using orchid_backend_net.Application.Seedling.CreateSeedling;
using orchid_backend_net.Application.Seedling.DeleteSeedling;
using orchid_backend_net.Application.Seedling.GetAllSeedling;
using orchid_backend_net.Application.Seedling.GetSeedlingInfor;
using orchid_backend_net.Application.Seedling.UpdateSeedling;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers.Seedling
{
    [Route("api/seedling")]
    [ApiController]
    public class SeedlingController(ISender _sender, ILogger<SeedlingController> _logger) : ControllerBase
    {

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<PageResult<SeedlingDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<PageResult<SeedlingDTO>>>> GetSeedlings(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] string? byMother,
            [FromQuery] string? byFather,
            [FromQuery] string? searchTerm,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _sender.Send(new GetAllSeedlingQuery(pageNumber, pageSize, byMother, byFather, searchTerm), cancellationToken);
                _logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<PageResult<SeedlingDTO>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<SeedlingDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<SeedlingDTO>>> GetSeedlingInfor(
            [FromRoute] string id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _sender.Send(new GetSeedlingInforQuery(id), cancellationToken);
                _logger.LogInformation("Received GET request for seedling {Id} at {Time}", id, DateTime.UtcNow);
                return Ok(new JsonResponse<SeedlingDTO>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GET request for seedling {Id} at {Time}", id, DateTime.UtcNow);
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
        public async Task<ActionResult<JsonResponse<string>>> CreateSeedling([FromBody] CreateSeedlingCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _sender.Send(command, cancellationToken);
                _logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                return CreatedAtAction(nameof(CreateSeedling), new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi tạo với lỗi sau: " + ex.Message);
            }
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateSeedling([FromBody] UpdateSeedlingCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _sender.Send(command, cancellationToken);
                _logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi cập nhật với lỗi sau: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSeedling([FromRoute] string id, CancellationToken cancellationToken)
        {
            try
            {
                await _sender.Send(new DeleteSeedlingCommand(id), cancellationToken);
                _logger.LogInformation("Received DELETE request for seedling {Id} at {Time}", id, DateTime.UtcNow);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing DELETE request for seedling {Id} at {Time}", id, DateTime.UtcNow);
                return BadRequest("Lỗi khi xóa với lỗi sau: " + ex.Message);
            }
        }
    }
}
