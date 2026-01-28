using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Characteristics.Dto;
using orchid_backend_net.Application.Characteristics.UseCase.GetAllCharacteristic;
using orchid_backend_net.Application.Characteristics.UseCase.GetCharacteristicById;
using orchid_backend_net.Application.Common.Pagination;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// characteristic controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/characteristics")] // Fixed: RESTful convention uses plural form
    [ApiController]
    public class CharacteristicController(ISender sender, ILogger<CharacteristicController> logger) : BaseController(sender)
    {
        /// <summary>
        /// using to get all characteristic when create seedling
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<CharacteristicDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCharacteristics([FromQuery] GetAllCharacteristicQuery query, CancellationToken cancellationToken = default)
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
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// get characteristic by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CharacteristicDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCharacteristicById([FromRoute] string id, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var query = new GetCharacteristicByIdQuery() { CharacteristicId = id };
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting characteristics.");
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }
    }
}
