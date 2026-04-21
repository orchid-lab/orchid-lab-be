using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.Chemical;
using orchid_backend_net.Application.Chemicals.Dto;
using orchid_backend_net.Application.Chemicals.UseCase.CreateChemicals;
using orchid_backend_net.Application.Chemicals.UseCase.DeleteChemical;
using orchid_backend_net.Application.Chemicals.UseCase.GetAllChemicals;
using orchid_backend_net.Application.Chemicals.UseCase.GetChemicalById;
using orchid_backend_net.Application.Chemicals.UseCase.UpdateChemical;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.MethodStageDefinition.Dto;
using orchid_backend_net.Application.MethodStageDefinition.UseCase.CreateMethodStageDefinition;
using orchid_backend_net.Application.MethodStageDefinition.UseCase.DeleteMethodStageDefinition;
using orchid_backend_net.Application.MethodStageDefinition.UseCase.GetAllMethodStageDefinition;
using orchid_backend_net.Application.MethodStageDefinition.UseCase.GetMethodStageDefinitionById;
using orchid_backend_net.Application.MethodStageDefinition.UseCase.UpdateMethodStageDefinition;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// method stage definition api
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/MethodStageDefinition")]
    [ApiController]
    public class MethodStageDefinitionController(ISender sender, ILogger<MethodStageDefinitionController> logger) : BaseController(sender)
    {
        /// <summary>
        /// get all method stage definition, use for method stage details
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<MethodStageDefinitionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMethodStageDefinition([FromQuery] GetAllMethodStageDefinitionQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting all method stage definition.");
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// get method stage definition by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MethodStageDefinitionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMethodStageDefinitionById([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var query = new GetMethodStageDefinitionByIdQuery() { Id = id };
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting method stage definition.");
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }
    }
}
