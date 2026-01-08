using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Chemicals.Dto;
using orchid_backend_net.Application.Chemicals.UseCase.GetChemicalById;
using orchid_backend_net.Application.Chemicals.UseCase.GetAllChemicals;
using orchid_backend_net.Application.Common.Pagination;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// chemical api
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/chemical")]
    [ApiController]
    public class ChemicalController(ISender sender, ILogger<ChemicalController> logger) : BaseController(sender)
    {
        /// <summary>
        /// get all chemical, use for add task attribute
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<ChemicalDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllChemical([FromQuery] GetAllChemicalsQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting all chemical.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }

        /// <summary>
        /// get chemical by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ChemicalDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCharacteristicById([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var query = new GetChemicalByIdQuery() { ChemicalId = id };
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting chemical.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
