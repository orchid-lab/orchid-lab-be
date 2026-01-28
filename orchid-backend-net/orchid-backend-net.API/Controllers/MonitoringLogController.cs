using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.MonitoringLog.Dto.AnalyticResult;
using orchid_backend_net.Application.MonitoringLog.Dto.MonitoringLog;
using orchid_backend_net.Application.MonitoringLog.UseCase.Analyze;
using orchid_backend_net.Application.MonitoringLog.UseCase.GetAllMonitoring;
using orchid_backend_net.Application.MonitoringLog.UseCase.GetMonitoringLogById;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Infrastructure.Service;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// monitoring log api controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/monitoring-logs")] // Fixed: RESTful convention uses plural form
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
    }
}
