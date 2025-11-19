using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Element;
using orchid_backend_net.Application.Element.CreateElement;
using orchid_backend_net.Application.Element.DeleteElement;
using orchid_backend_net.Application.Element.GetAllElement;
using orchid_backend_net.Application.Element.GetElementInfor;
using orchid_backend_net.Application.Element.UpdateElement;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers.Element
{
    [Route("api/element")]
    [ApiController]
    public class ElementController(ISender _sender, ILogger<ElementController> _logger) : BaseController(_sender)
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<PageResult<ElementDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<PageResult<ElementDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<PageResult<ElementDTO>>>> GetAllElements(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(new GetAllElementQuery(pageNumber, pageSize), cancellationToken);
                _logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<PageResult<ElementDTO>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }



        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<ElementDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<ElementDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<ElementDTO>>> GetElementInfor(
            [FromRoute] string id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(new GetElementInforQuery { ID = id }, cancellationToken);
                _logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<ElementDTO>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
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
        public async Task<ActionResult<JsonResponse<string>>> UpdateElement(
            [FromBody]UpdateElementCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(command, cancellationToken);
                _logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi cập nhật với lỗi sau: " + ex.Message);
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
        public async Task<ActionResult<JsonResponse<string>>> CreateElement(
            [FromBody]CreateElementCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(command, cancellationToken);
                _logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi tạo với lỗi sau: " + ex.Message);
            }
        }
        [HttpDelete]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        public async Task<ActionResult<JsonResponse<string>>> DeleteElement(
            [FromBody]DeleteElementCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(command, cancellationToken);
                _logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing DELETE request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi xóa với lỗi sau: " + ex.Message);
            }
        }
    }
}
