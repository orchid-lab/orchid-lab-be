using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Method.Dto.SampleRequirement;
using orchid_backend_net.Application.SampleRequirementDefinition.UseCase.GetAllSampleRequirementDefinition;
using orchid_backend_net.Application.SampleRequirementDefinition.UseCase.GetSampleRequirementDefinitionById;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// sample requirement api only using for get all
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/sample-requirement")]
    [ApiController]
    public class SampleRequirementDefinitionController(ISender sender, ILogger<SampleRequirementDefinitionController> logger) : BaseController(sender)
    {
        /// <summary>
        /// using to get all sample requirement when creating monitoring logs, methods
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<SampleRequirementDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCharacteristics([FromQuery] GetAllSampleRequirementDefinitionQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting all characteristics.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }

        /// <summary>
        /// get sample requirement definition by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SampleRequirementDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCharacteristicById([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var query = new GetSampleRequirementDefinitionByIdQuery(id);
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting characteristics.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
