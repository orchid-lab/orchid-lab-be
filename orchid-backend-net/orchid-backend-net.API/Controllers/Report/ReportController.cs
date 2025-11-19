using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Images.Create;
using orchid_backend_net.Application.Report;
using orchid_backend_net.Application.Report.CreateReport;
using orchid_backend_net.Application.Report.DeleteReport;
using orchid_backend_net.Application.Report.ExportReportPDF;
using orchid_backend_net.Application.Report.GetAllReport;
using orchid_backend_net.Application.Report.GetReportInfor;
using orchid_backend_net.Application.Report.ReviewReport;
using orchid_backend_net.Application.Report.UpdateReport;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers.Report
{
    [Route("api/report")]
    [ApiController]
    public class ReportController(ISender sender, ILogger<ReportController> logger) : BaseController(sender)
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<ReportDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<ReportDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<PageResult<ReportDTO>>>> GetAll(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] string? technicianId,
            [FromQuery] string? experimentLogId,
            [FromQuery] string? stageId,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(new GetAllReportQuery(pageNumber, pageSize, technicianId, experimentLogId, stageId), cancellationToken);
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<PageResult<ReportDTO>>(result));
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<ReportDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<ReportDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<ReportDTO>>> GetInfor(
           [FromQuery] string id,
           CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(new GetReportInforQuery(id), cancellationToken);
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<ReportDTO>(result));
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
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
            [FromBody] CreateReportCommand command,
            CancellationToken cancellationToken)
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

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Update(
            [FromBody] UpdateReportCommand command,
            CancellationToken cancellationToken)
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


        [HttpPut("review-report-change")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> ReviewReportChange(
            [FromBody] ReviewReportCommand command,
            CancellationToken cancellationToken)
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
            [FromBody] DeleteReportCommand command,
            CancellationToken cancellationToken)
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

        [HttpGet("export-pdf/{experimentLogId}")]
        public async Task<IActionResult> ExportReportToPdf([FromRoute] string experimentLogId, CancellationToken cancellationToken)
        {
            try
            {
                var pdfBytes = await sender.Send(new ExportReportPdfCommand(experimentLogId), cancellationToken);
                return File(pdfBytes, "application/pdf", $"Report_{experimentLogId}.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while exporting report to PDF at {Time}", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, "Có lỗi xảy ra trong quá trình xuất file");
            }
        }
    }
}
