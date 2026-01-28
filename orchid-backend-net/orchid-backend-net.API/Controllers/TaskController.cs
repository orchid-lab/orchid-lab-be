using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Tasks.Dto.Task;
using orchid_backend_net.Application.Tasks.UseCase.GetTaskById;
using orchid_backend_net.Application.Tasks.UseCase.ChangeTaskStatus;
using orchid_backend_net.Application.Tasks.UseCase.ConvertTaskTemplateToTodoTask;
using orchid_backend_net.Application.Tasks.UseCase.CreateTask;
using orchid_backend_net.Application.Tasks.UseCase.DeleteTask;
using orchid_backend_net.Application.Tasks.UseCase.GetAllTask;
using orchid_backend_net.Application.Tasks.UseCase.UpdateTask;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// task controller
    /// </summary>
    [Route("api/tasks")]
    [ApiController]
    public class TaskController(ILogger<TaskController> logger, ISender sender) : BaseController(sender)
    {
        /// <summary>
        /// use for get all tasks, not need for authorization
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<PageResult<TaskDto>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<PageResult<TaskDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTasks([FromQuery] GetAllTaskQuery query, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// use for get task by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<TaskDetailDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<TaskDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTaskById([FromRoute] string id, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetTaskByIdQuery() { Id = id }, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// create template/to-do task, only researcher can be used,
        /// CreateTaskAssignmentDto can be null in case want to create task template
        /// Task attribute can be null in case you need to create task like observation
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Create([FromBody] CreateTaskCommand command, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Tạo thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// researcher using this to convert template task to to-do task
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPost("template-conversion")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> TaskTemplateConverted([FromBody] ConvertTaskTemplateToToDoTaskCommand command, CancellationToken cancellationToken)
        {
            try
            {   
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Tạo thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// researcher using this api to update task information, not re-assign technician
        /// update task attribute can be null
        /// update task assignment can be null
        /// create task attribute can be null
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Update([FromBody] UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// technician use for updated status task to in-progess and waiting for approval
        /// researcher use for updated task status to completed in time, completed out time and in progress
        /// </summary>
        /// <remarks>
        /// Technician can update status to:
        /// <ul>
        /// <li><c>InProgress</c> - when technician is confirm doing task</li>
        /// <li><c>DeclinedByTechnician</c> - when technician decline doing task</li>
        /// <li><c>WaitingForApproval</c> - when technician need researcher confirm their task complete</li>
        /// </ul>
        /// Researcher can update status to:
        /// <ul>
        /// <li><c>CompletedInTime</c> - when technician completed task in time</li>
        /// <li><c>CompletedOutTime</c> - when technician completed task out of time</li>
        /// <li><c>ReworkRequired</c> - when technician is need to rework</li>
        /// </ul>
        /// </remarks>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut("change-task-status")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> ChangeStatus([FromBody] ChangeTaskStatusCommand command, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// researcher using this api to delete task
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpDelete]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Delete([FromBody] DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Xóa thất bại", Detail = ex.Message });
            }
        }
    }
}
