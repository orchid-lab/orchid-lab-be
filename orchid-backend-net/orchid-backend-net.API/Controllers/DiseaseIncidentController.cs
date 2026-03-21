using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.Application.DiseaseIncident.UseCase.GetByExperimentLog;
using orchid_backend_net.Application.DiseaseIncident.UseCase.ReviewIncident;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.DiseaseIncident;
using orchid_backend_net.Application.DiseaseIncident.Dto;
using orchid_backend_net.Application.Common.Pagination;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// api controller for disease incident, trigger when there is a disease incident in the experiment,
    /// only for researcher and technician roles, researcher can review the incident and add action for the incident,
    /// technician can only add action for the incident
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/disease-incidents")]
    [ApiController]
    public class DiseaseIncidentController(ISender sender, ILogger<DiseaseIncidentController> logger) : BaseController(sender)
    {
        /// <summary>
        /// review the incident in sample
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}/review")]
        [Authorize(Roles = "Researcher,Technician")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Review([FromRoute] string id, [FromBody] ReviewDiseaseIncidentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received PUT review request for incident {Id} at {Time}", id, DateTime.UtcNow);
                var command = new ReviewDiseaseIncidentCommand(id, request.IsConfirmed, request.Note);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in review incident {Id} at {Time}", id, DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Xử lý thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// get all
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="experimentLogId"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(JsonResponse<PageResult<DiseaseIncidentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByExperimentLog(
            [FromQuery] int pageNo,
            [FromQuery] int pageSize,
            [FromQuery] string? experimentLogId, 
            [FromQuery] int? status, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received get incidents for experimentLogId {Id} at {Time}", experimentLogId, DateTime.UtcNow);
                var statusEnum = status.HasValue ? (Domain.Common.Enum.DiseaseIncidentStatus?)status.Value : null;
                var query = new GetDiseaseIncidentsByExperimentLogQuery(pageNo, pageSize,experimentLogId, statusEnum);
                var result = await Sender.Send(query, cancellationToken);
                return Ok(new JsonResponse<PageResult<DiseaseIncidentDto>>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in get incidents for experimentLogId {Id} at {Time}", experimentLogId, DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }
    }
}
