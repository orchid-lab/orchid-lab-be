using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.StageRequirementDefinition.Dto.StageRequirementDefinitionDto;
using orchid_backend_net.Application.StageRequirementDefinition.UseCase.GetAll;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// api for stage requirement
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/stage-requirement-definition")]
    [ApiController]
    public class StageRequirementDefinitionController(ISender sender, ILogger<StageRequirementDefinitionController> logger) : BaseController(sender)
    {
        /// <summary>
        /// get all stage requirement
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sampleStageId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<StageRequirementDefinitionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStageRequirementDefinition(
            [FromQuery] int pageNo,
            [FromQuery] int pageSize,
            [FromQuery] string? sampleStageId,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetAllStageRequirementDefinitionQuery(pageNo, pageSize, sampleStageId), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting all chemical.");
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }
    }
}
