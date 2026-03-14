using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.ExperimentLog;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.ExperimentLog.Dto.ExperimentLog;
using orchid_backend_net.Application.ExperimentLog.UseCase.CreateExperimentLog;
using orchid_backend_net.Application.ExperimentLog.UseCase.DeleteExperimentLog;
using orchid_backend_net.Application.ExperimentLog.UseCase.GetAllExperimentLog;
using orchid_backend_net.Application.ExperimentLog.UseCase.GetExperimentLogById;
using orchid_backend_net.Application.ExperimentLog.UseCase.UpdateExperimentLogInformation;
using orchid_backend_net.Application.ExperimentLog.UseCase.UpdateExperimentLogStatus;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// experiment log api controller 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/experiment-logs")]
    [ApiController]
    public class ExperimentLogController(ISender sender, ILogger<ExperimentLogController> logger) : BaseController(sender)
    {
        /// <summary>
        /// get all experiment logs with pagination and filtering
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<ExperimentLogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllExperimentLogQuery query,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// get experiment log detail by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExperimentLogDetailDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetExperimentLogByIdQuery(id), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// use this api as researcher to create experiment log
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPost]
        [Authorize(Roles = "Researcher")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> CreateExperimentLog([FromBody] CreateExperimentLogCommand command, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Tạo thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// update experiment log information by id
        /// only researcher use this api
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut("{id}")]
        [Authorize(Roles = "Researcher")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateExperimentLogInformation([FromRoute] string id, [FromBody] UpdateExperimentLogInformationDto dto, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var command = new UpdateExperimentLogInformationCommand(id, dto.Name, dto.Notes, dto.ExpectedSampleCount, dto.Objective);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// use this api as researcher to update experiment log by status 
        /// </summary>
        /// <remarks>
        /// Use the following status enum values:
        /// <para>
        /// <ul>
        /// <li><c>InProgress</c> — Technician pressed the "Start Experiment" button.</li>
        /// <li><c>WaitingForChangeStage</c> — Technician finished a stage and requested a stage change; researcher confirmation is required.</li>
        /// <li><c>ConfirmChangeStage</c> — Researcher confirmed the stage change. After this, the status becomes <c>InProgress</c> and the experiment's current stage is incremented by 1.</li>
        /// <li><c>Completed</c> — Technician pressed the "Complete Experiment" button; the experiment is finished.</li>
        /// </ul>
        /// </para>
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Researcher, Technician")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateExperimentLogStatus([FromRoute] string id, [FromBody] UpdateExperimentLogStatusDto dto, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var command = new UpdateExperimentLogStatusCommand(id, dto.Status, dto.BatchId, dto.Reason, dto.Conclusion, dto.Issues, dto.Recommendations);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// researcher only use this api to destroy experiment log if
        /// experiment log's sample are all infected with disease or experiment log is created by mistake
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Researcher")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> DestroyExperimentLog([FromRoute] string id, [FromBody] string? reason, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var command = new DeleteExperimentLogCommand(id, reason);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Hủy thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// technician use this api to cancel experiment log if had any reason to stop the experiment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("cancel/{id}")]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> CancelExperimentLog([FromRoute] string id, [FromBody] string? reason, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var command = new Application.ExperimentLog.UseCase.CancelExperimentLog.CancelExperimentLogCommand(id, reason);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Hủy thất bại", Detail = ex.Message });
            }
        }
    }
}
