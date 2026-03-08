using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.MonitoringLog;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.MonitoringLog.Dto.AnalyticResult;
using orchid_backend_net.Application.MonitoringLog.Dto.LogDetail;
using orchid_backend_net.Application.MonitoringLog.Dto.MonitoringLog;
using orchid_backend_net.Application.MonitoringLog.UseCase.Analyze;
using orchid_backend_net.Application.MonitoringLog.UseCase.ApproveMonitoringLog;
using orchid_backend_net.Application.MonitoringLog.UseCase.CreateMonitoringLog;
using orchid_backend_net.Application.MonitoringLog.UseCase.GetAllMonitoring;
using orchid_backend_net.Application.MonitoringLog.UseCase.GetMonitoringLogById;
using orchid_backend_net.Application.MonitoringLog.UseCase.RejectMonitoringLog;
using orchid_backend_net.Application.MonitoringLog.UseCase.SubmitMonitoringLog;
using orchid_backend_net.Application.MonitoringLog.UseCase.UpdateMonitoringLogDetail;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Infrastructure.Service;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// monitoring log api controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/monitoring-log")]
    [ApiController]
    public class MonitoringLogController(ISender sender, ILogger<MonitoringLogController> logger) : BaseController(sender)
    {
        /// <summary>
        /// get all monitoring logs
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="nameSearchTerm"></param>
        /// <param name="TechnicianId"></param>
        /// <param name="sampleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<MonitoringLogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMonitoringLogs(
            [FromQuery] int pageNo,
            [FromQuery] int pageSize,
            [FromQuery] string? nameSearchTerm,
            [FromQuery] string? TechnicianId,
            [FromQuery] string? sampleName,
            CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetAllMonitoringLogQuery(pageNo, pageSize, TechnicianId, sampleName, nameSearchTerm), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// get monitoring log by id to view detail
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MonitoringLogDetailDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMonitoringLogById(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetMonitoringLogById(id), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// analyze the orchid stage and disease 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(Roles = "Technician, Researcher")]
        [HttpPost("analysis")]
        [ProducesResponseType(typeof(AnalyticResultAfterAnalysisDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Analytic(
            IFormFile image, CancellationToken cancellationToken)
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

                var command = new AnalyzeOrchidImageCommand(image.FileName, resizedBytes);
                var result = await Sender.Send(command, cancellationToken);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Phân tích thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// create monitoring log for sample
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(Roles = "Technician")]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> Create(
            [FromBody] CreateMonitoringLogCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Tạo thất bại thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Technician manually submits draft monitoring log for researcher approval.
        /// Only needed if monitoring log was created with submitImmediately=false.
        /// Also used to resubmit rejected monitoring logs after updating details.
        /// </summary>
        /// <param name="id">Monitoring log ID</param>
        [HttpPatch("{id}/submit")]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SubmitMonitoringLog(
            [FromRoute] string id)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new SubmitMonitoringLogCommand(id));
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Researcher approves monitoring log.
        /// Sets this log as newest (IsNewest=true) and marks all other approved logs as old.
        /// </summary>
        /// <param name="id">Monitoring log ID</param>
        [HttpPatch("{id}/approve")]
        [Authorize(Roles = "Researcher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApproveMonitoringLog(
            [FromRoute]string id)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new ApproveMonitoringLogCommand(id));
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Researcher rejects monitoring log with reason.
        /// Technician can then update log details and resubmit.
        /// </summary>
        /// <param name="id">Monitoring log ID</param>
        /// <param name="reason">Rejection reason</param>
        [HttpPatch("{id}/reject")]
        [Authorize(Roles = "Researcher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectMonitoringLog(
            [FromRoute] string id,
            [FromBody] string reason)
        {
            try
            {
                logger.LogInformation("Received Patch request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new RejectMonitoringLogCommand(id, reason));
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing Patch request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Technician updates log details after rejection.
        /// Can only update monitoring logs with status: Rejected.
        /// After updating, technician must resubmit for approval.
        /// </summary>
        /// <param name="id">Monitoring log ID</param>
        /// <param name="request">Updated log details</param>
        [HttpPatch("{id}/update-details")]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMonitoringLogDetails(
            string id,
            [FromBody] UpdateMonitoringLogDetailDto request)
        {
            try
            {
                logger.LogInformation("Received Patch request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new UpdateMonitoringLogDetailCommand(id, request.UpdatedLogDetails));
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing Patch request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại thất bại", Detail = ex.Message });
            }
        }
    }
}
