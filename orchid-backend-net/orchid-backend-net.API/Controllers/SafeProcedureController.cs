using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.SafeProcedure;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedure;
using orchid_backend_net.Application.SafeProcedure.UseCase.Create;
using orchid_backend_net.Application.SafeProcedure.UseCase.Delete;
using orchid_backend_net.Application.SafeProcedure.UseCase.GetAll;
using orchid_backend_net.Application.SafeProcedure.UseCase.GetById;
using orchid_backend_net.Application.SafeProcedure.UseCase.Update;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// safe procedure api controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/safe-procedure")]
    [ApiController]
    public class SafeProcedureController(ISender sender, ILogger<SafeProcedureController> logger) : BaseController(sender)
    {
        /// <summary>
        /// get all safe procedure with pagination and optional search term
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="searchTerm"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<SafeProcDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageSize,
            [FromQuery] int pageNumber,
            [FromQuery] string? searchTerm,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetAllSafeProcedureQuery(pageNumber, pageSize, searchTerm), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// get detail of a safe procedure by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SafeProcedureDetailDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetSafeProcedureByIdQuery(id), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// create new safe procedure with admin role
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>>
            CreateSafeProcedure([FromBody] CreateSafeProcedureCommand command, CancellationToken cancellationToken)
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
                return BadRequest(new ProblemDetails { Title = "Tạo quy trình an toàn thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// update safe procedure by id with admin role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateSafeProcedure(
            [FromRoute] string id,
            [FromBody] UpdateSafeProcedureDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                if (id is null)
                {
                    return BadRequest(new ProblemDetails { Title = "ID để trống", Detail = "ID không được để trống" });
                }
                var result = await Sender.Send(new UpdateSafeProcedureCommand(id,
                    dto.ProcedureName,
                    dto.Description,
                    dto.ProcedureType,
                    dto.SafeProcedureSteps), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Cập nhật quy trình an toàn thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// delete safe procedure by id with admin role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> DeleteSafeProcedure(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                if (id is null)
                {
                    return BadRequest(new ProblemDetails { Title = "ID để trống", Detail = "ID không được để trống" });
                }
                var result = await Sender.Send(new DeleteSafeProcedureCommand(id), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Xoá quy trình an toàn thất bại", Detail = ex.Message });
            }
        }
    }
}
