using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.Config;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.LabConfig.Dto.LabConfig;
using orchid_backend_net.Application.LabConfig.UseCase.Create;
using orchid_backend_net.Application.LabConfig.UseCase.Delete;
using orchid_backend_net.Application.LabConfig.UseCase.GetAll;
using orchid_backend_net.Application.LabConfig.UseCase.GetById;
using orchid_backend_net.Application.LabConfig.UseCase.Update;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// Config Controller api
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/config")]
    [ApiController]
    public class ConfigController(ISender sender, ILogger<ConfigController> logger) : BaseController(sender)
    {
        /// <summary>
        /// get all config
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchTerm"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<ConfigDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConfigs(
            [FromQuery] int pageNo,
            [FromQuery] int pageSize,
            [FromQuery] string? searchTerm,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetAllConfigLabQuery(pageNo, pageSize, searchTerm), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// get config by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ConfigDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConfigById(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetConfigByIdQuery(id), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// create new config
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> Create(
            [FromBody] CreateLabConfigCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return CreatedAtAction(nameof(GetConfigById), new { id = result }, new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Tạo cấu hình thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// update config in server
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> Update(
            [FromRoute] string id,
            [FromBody] UpdateConfigDto command,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                if (id is null)
                {
                    return BadRequest(new ProblemDetails { Title = "ID không khớp" });
                }
                var result = await Sender.Send(new UpdateConfigCommand(id, command.ConfigName, command.Key, command.Value), cancellationToken);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Cập nhật cấu hình thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Delete config by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> Delete(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new DeleteConfigCommand(id), cancellationToken);
                return Ok(new JsonResponse<string>($"Config with ID {result} deleted."));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Xóa cấu hình thất bại", Detail = ex.Message });
            }
        }
    }
}
