using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.LabRoom.Dto;
using orchid_backend_net.Application.LabRoom.UseCase.GetAllLabRoom;
using orchid_backend_net.Application.LabRoom.UseCase.GetLabRoomByIdQuery;
using orchid_backend_net.Application.MethodStageDefinition.UseCase.GetAllMethodStageDefinition;
using orchid_backend_net.Application.MethodStageDefinition.UseCase.GetMethodStageDefinitionById;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// chemical api
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/LabRoom")]
    [ApiController]
    public class LabRoomController(ISender sender, ILogger logger) : BaseController(sender)
    {
        /// <summary>
        /// get all lab room, use for btach drop down list
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<LabRoomDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLabRoom([FromQuery] GetAllLabRoomQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting all Lab room.");
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
        [ProducesResponseType(typeof(LabRoomDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLabRoomById([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var query = new GetLabRoomByIdQuery() { Id = id };
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting lab room.");
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }
    }
}
