using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Report.CreateReport;
using orchid_backend_net.Application.Report.DeleteReport;
using orchid_backend_net.Application.Report.UpdateReport;
using orchid_backend_net.Application.TissueCultureBatch;
using orchid_backend_net.Application.TissueCultureBatch.CreateTissueCultureBatch;
using orchid_backend_net.Application.TissueCultureBatch.DeleteTissueCultureBatch;
using orchid_backend_net.Application.TissueCultureBatch.GetAllTissueCultureBatch;
using orchid_backend_net.Application.TissueCultureBatch.GetTissueCultureBatchInfor;
using orchid_backend_net.Application.TissueCultureBatch.UpdateTissueCultureBatch;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers.TissueCultureBatch
{
    [Route("api/tissue-culture-batch")]
    [ApiController]
    public class TissueCultureBatchController(ISender _sender, ILogger<TissueCultureBatchController> _logger) : BaseController(_sender)
    {

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<TissueCultureBatchDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<TissueCultureBatchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<PageResult<TissueCultureBatchDTO>>>> GetAll(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            CancellationToken cancellationToken)
        {
            try
            {

                var result = await this._sender.Send(new GetAllTissueCultureBatchQuery(pageNumber, pageSize), cancellationToken);
                _logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<PageResult<TissueCultureBatchDTO>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<TissueCultureBatchDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<TissueCultureBatchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<TissueCultureBatchDTO>>> GetInfor(
           [FromRoute] string id,
           CancellationToken cancellationToken)
        {
            try
            {
                var result = await _sender.Send(new GetTissueCultureBatchInforQuery(id), cancellationToken);
                _logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<TissueCultureBatchDTO>(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
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
            [FromBody] CreateTissueCultureBatchCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _sender.Send(command, cancellationToken);
                _logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi tạo với lỗi sau: " + ex.Message);
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
            [FromBody] UpdateTissueCultureBatchCommand command,
            CancellationToken cancellationToken)
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

        [HttpDelete]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Delete(
            [FromBody] DeleteTissueCultureBatchCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _sender.Send(command, cancellationToken);
                _logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing DELETE request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi xóa với lỗi sau: " + ex.Message);
            }
        }

    }
}
