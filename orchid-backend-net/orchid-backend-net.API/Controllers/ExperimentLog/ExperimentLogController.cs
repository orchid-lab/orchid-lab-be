using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.ExperimentLog;
using orchid_backend_net.Application.ExperimentLog.CreateExperimentLog;
using orchid_backend_net.Application.ExperimentLog.DeleteExperimentLog;
using orchid_backend_net.Application.ExperimentLog.GetAllExperimentLog;
using orchid_backend_net.Application.ExperimentLog.GetExperimentLogInfor;
using orchid_backend_net.Application.ExperimentLog.StateChangeExperimentLog;
using orchid_backend_net.Application.ExperimentLog.UpdateExperimentLog;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers.ExperimentLog
{
    [Route("api/experimentlog")]
    [ApiController]
    public class ExperimentLogController(ISender _sender, ILogger<ExperimentLogController> _logger) : BaseController(_sender)
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<ExperimentLogDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<ExperimentLogDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<PageResult<ExperimentLogDTO>>>> GetAllExperimentLog(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] string? filter,
            [FromQuery] string? searchTerm,
            CancellationToken cancellationToken)
        {
            try
            {

                var result = await this._sender.Send(new GetAllExperimentLogQuery(pageNumber, pageSize, filter, searchTerm), cancellationToken);
                _logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<PageResult<ExperimentLogDTO>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<ExperimentLogDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<ExperimentLogDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<ExperimentLogDTO>>> GetExperimentLogInfor(
            [FromRoute] string id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(new GetExperimentLogInforQuery { ID = id }, cancellationToken);
                _logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<ExperimentLogDTO>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
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
        public async Task<ActionResult<JsonResponse<string>>> UpdateExperimentLog(
            UpdateExperimentLogCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(command, cancellationToken);
                _logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi cập nhật với lỗi sau: " + ex.Message);
            }
        }
        [HttpPut("stage-changer")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> StateChange(
            StateChangeExperimentLogCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(command, cancellationToken);
                _logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi cập nhật với lỗi sau: " + ex.Message);
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
        public async Task<ActionResult<JsonResponse<string>>> CreateExperimentLog(
            CreateExperimentLogCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(command, cancellationToken);
                _logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi tạo với lỗi sau: " + ex.Message);
            }
        }
        [HttpDelete]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<JsonResponse<string>>> DeleteExperimentLog(
            DeleteExperimentLogCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(command, cancellationToken);
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
