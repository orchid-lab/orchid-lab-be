using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.TaskTemplate;
using orchid_backend_net.Application.TaskTemplate.CreateTaskTemplate;
using orchid_backend_net.Application.TaskTemplate.DeleteTaskTemplate;
using orchid_backend_net.Application.TaskTemplate.GetAllTaskTemplate;
using orchid_backend_net.Application.TaskTemplate.GetTaskTemplateInfor;
using orchid_backend_net.Application.TaskTemplate.UpdateTaskTemplate;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers.TaskTemplate
{
    [Route("api/TaskTemplate")]
    [ApiController]
    public class TaskTemplateController(ISender sender, ILogger<TaskTemplateController> logger) : BaseController(sender)
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<TaskTemplateDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<TaskTemplateDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<PageResult<TaskTemplateDTO>>>> GetAll(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] string? filter,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(new GetAllTaskTemplateQuery(pageNumber, pageSize, filter), cancellationToken);
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<PageResult<TaskTemplateDTO>>(result));
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<TaskTemplateDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<TaskTemplateDTO>>> GetInfor(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(new GetTaskTemplateInforQuery(id), cancellationToken);
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<TaskTemplateDTO>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> CreateTaskTemplate([FromBody] CreateTaskTemplateCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(command, cancellationToken);
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi tạo với lỗi sau: " + ex.Message);
            }
        }
        [HttpDelete]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> DeleteTaskTemplate(DeleteTaskTemplateCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(command, cancellationToken);
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing DELETE request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi xóa với lỗi sau: " + ex.Message);
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
        public async Task<ActionResult<JsonResponse<string>>> UpdateTaskTemplate([FromBody] UpdateTaskTemplateCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this._sender.Send(command, cancellationToken);
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi cập nhật với lỗi sau: " + ex.Message);
            }
        }
    }
}
