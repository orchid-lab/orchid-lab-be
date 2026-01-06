using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Materials.Dto;
using orchid_backend_net.Application.Materials.GetAllMaterials;
using orchid_backend_net.Application.Materials.GetMaterialById;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// material api
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/material")]
    [ApiController]
    public class MaterialController(ISender sender, ILogger<MaterialController> logger) : BaseController(sender)
    {
        /// <summary>
        /// get all material, use for add task attribute
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<MaterialDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllChemical([FromQuery] GetAllMaterialsQuery query, CancellationToken cancellationToken = default)
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
        /// get material by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCharacteristicById([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var query = new GetMaterialByIdQuery() { MaterialId = id };
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
