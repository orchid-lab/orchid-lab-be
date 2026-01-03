using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Characteristics.Dto;
using orchid_backend_net.Application.Characteristics.GetAllCharacteristic;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Seedling.GetAllSeedlings;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// characteristic controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/characteristic")]
    [ApiController]
    public class CharacteristicController(ISender sender, ILogger<CharacteristicController> logger) : BaseController(sender)
    {
        /// <summary>
        /// using to get all characteristic when create seedling
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<CharacteristicDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCharacteristics([FromQuery] GetAllSeedlingsQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting all characteristics.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
