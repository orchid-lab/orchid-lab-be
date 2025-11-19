using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.LabRoom;
using orchid_backend_net.Application.LabRoom.CreateLabRoom;
using orchid_backend_net.Application.LabRoom.DeleteLabRoom;
using orchid_backend_net.Application.LabRoom.GetAllLabRoom;
using orchid_backend_net.Application.LabRoom.GetLabRoomInfor;
using orchid_backend_net.Application.LabRoom.UpdateLabRoom;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers.LabRoom
{
    [Route("api/labroom")]
    [ApiController]
    public class LabRoomController(ISender sender, ILogger<LabRoomController> logger) : BaseController(sender)
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<LabRoomDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<LabRoomDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<PageResult<LabRoomDTO>>>> GetAllLabRoom(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(new GetAllLabRoomQuery(pageNumber, pageSize), cancellationToken);
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<PageResult<LabRoomDTO>>(result));
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<LabRoomDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<LabRoomDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<LabRoomDTO>>> GetAllLabRoom(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(new GetLabRoomInforQuery(id), cancellationToken);
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<LabRoomDTO>(result));
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> CreateLabRoom(
            [FromBody] CreateLabRoomCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(command, cancellationToken);
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi tạo với lỗi sau: " + ex.Message);
            }
        }

        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateLabRoom(
            [FromBody] UpdateLabRoomCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(command, cancellationToken);
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi cập nhật với lỗi sau: " + ex.Message);
            }
        }

        [HttpDelete]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> DeleteLabRoom(
            [FromBody] DeleteLabRoomCommand command, 
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(command, cancellationToken);
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing DELETE request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi xóa với lỗi sau: " + ex.Message);
            }
        }
    }
}
