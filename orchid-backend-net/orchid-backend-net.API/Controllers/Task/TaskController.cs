using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Tasks;
using orchid_backend_net.Application.Tasks.CreateTask;
using orchid_backend_net.Application.Tasks.DeleteTask;
using orchid_backend_net.Application.Tasks.GetAllTasks;
using orchid_backend_net.Application.Tasks.GetTaskInfor;
using orchid_backend_net.Application.Tasks.ReportTask;
using orchid_backend_net.Application.Tasks.UpdateTask;
using orchid_backend_net.Application.Tasks.UpdateTaskStatus;
using System.Net.Mime;

namespace orchid_backend_net.API.Controllers.Task
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController(ISender sender, ILogger<TaskController> logger) : BaseController(sender)
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<GetAllTaskQueryDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<GetAllTaskQueryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<PageResult<GetAllTaskQueryDto>>>> GetAll(
            [FromQuery] int pageNo,
            [FromQuery] int pageSize,
            [FromQuery] string? technicianId,
            [FromQuery] string? researcherId,
            [FromQuery] string? stageId,
            [FromQuery] string? experimentlogId,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(new GetAllTaskQuery(pageNo, pageSize, technicianId, researcherId, stageId, experimentlogId), cancellationToken);
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<PageResult<GetAllTaskQueryDto>>(result));
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<TaskDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<TaskDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<PageResult<TaskDTO>>>> GetTaskInfor(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(new GetTaskInforQuery(id), cancellationToken);
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                return Ok(new JsonResponse<TaskDTO>(result));
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "Error occurred while processing GET request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi lấy với lỗi sau: " + ex.Message);
            }
        }

        /// <summary>
        /// researcher create task
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<TaskDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<TaskDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Create(
            [FromBody] CreateTaskCommand command,
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
                logger.LogInformation(ex, "Error occurred while processing POST request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi tạo với lỗi sau: " + ex.Message);
            }
        }

        /// <summary>
        /// researcher update task
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<TaskDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<TaskDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Update(
            [FromBody] UpdateTaskCommand command,
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
                logger.LogInformation(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi cập nhật với lỗi sau: " + ex.Message);
            }
        }


        /// <summary>
        /// use when technician update task status
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("update-status")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<TaskDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<TaskDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateTaskStatus(
            [FromBody] UpdateTaskStatusCommand command,
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
                logger.LogInformation(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi cập nhật với lỗi sau: " + ex.Message);
            }
        }



        /// <summary>
        /// researcher update task
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("update-report-task")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateReportTask(
            IFormFile image,
            [FromForm] string description,
            [FromForm] string taskid,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                if (string.IsNullOrEmpty(taskid))
                    return BadRequest("Task ID is required.");
                if (image == null)
                    return BadRequest("No images provided.");
                using var stream = image.OpenReadStream();
                stream.Position = 0;
                var command = new ReportTaskCommand(stream, image.FileName, description, taskid);
                var imageResult = await sender.Send(command, cancellationToken);
                if (imageResult.Equals(false))
                {
                    return BadRequest("Failed to upload image: " + image.FileName);
                }
                return Ok(new JsonResponse<string>("Images uploaded successfully."));
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex, "Error occurred while processing PUT request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi cập nhật với lỗi sau: " + ex.Message);
            }
        }



        [HttpDelete]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<TaskDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(JsonResponse<TaskDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Delete(
            [FromBody] DeleteTaskCommand command,
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
                logger.LogInformation(ex, "Error occurred while processing DELETE request at {Time}", DateTime.UtcNow);
                return BadRequest("Lỗi khi xóa với lỗi sau: " + ex.Message);
            }
        }
    }
}
