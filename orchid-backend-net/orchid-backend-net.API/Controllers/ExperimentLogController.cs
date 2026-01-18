using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.ExperimentLog.Dto.ExperimentLog;
using orchid_backend_net.Application.ExperimentLog.UseCase.CreateExperimentLog;
using orchid_backend_net.Application.ExperimentLog.UseCase.GetAllExperimentLog;
using orchid_backend_net.Application.ExperimentLog.UseCase.GetExperimentLogById;

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
        /// get all experiment log
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<ExperimentLogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery]int pageSize, 
            [FromQuery]int pageNo,
            [FromQuery]string? nameSearchTerm,
            [FromQuery]string? methodNameSearchTerm,
            [FromQuery]int? currentStageOrder,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetAllExperimentLogQuery(pageNo, pageSize, nameSearchTerm, methodNameSearchTerm, currentStageOrder), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
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
                throw new InvalidOperationException(ex.Message);
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
            logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
            var result = await Sender.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
