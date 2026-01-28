using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Chemicals.Dto;
using orchid_backend_net.Application.Chemicals.UseCase.GetChemicalById;
using orchid_backend_net.Application.Chemicals.UseCase.GetAllChemicals;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Chemicals.UseCase.CreateChemicals;
using orchid_backend_net.Application.Chemicals.UseCase.DeleteChemical;
using Microsoft.AspNetCore.Authorization;
using orchid_backend_net.API.Dto.Chemical;
using orchid_backend_net.Application.Chemicals.UseCase.UpdateChemical;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// chemical api
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/chemicals")] // Fixed: RESTful convention uses plural form
    [ApiController]
    public class ChemicalController(ISender sender, ILogger<ChemicalController> logger) : BaseController(sender)
    {
        /// <summary>
        /// get all chemical, use for add task attribute
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<ChemicalDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllChemical([FromQuery] GetAllChemicalsQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting all chemical.");
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// get chemical by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ChemicalDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChemicalById([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var query = new GetChemicalByIdQuery() { ChemicalId = id };
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting chemical.");
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// create chemical only admin can use this
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> CreateChemical([FromBody] CreateChemicalCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return CreatedAtAction(nameof(GetChemicalById), new { id = result }, new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating chemical.");
                return BadRequest(new ProblemDetails { Title = "Tạo thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// delete chemical only admin can use this
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> DeleteChemical([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new DeleteChemicalCommand(id), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting chemical.");
                return BadRequest(new ProblemDetails { Title = "Xóa thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// update chemical only admin can use this
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateChemical(
            [FromRoute] int id,
            [FromBody] UpdateChemicalDto dto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new UpdateChemicalCommand(id, dto.Name, dto.Category, dto.Description, dto.Unit), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting chemical.");
                return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại", Detail = ex.Message });
            }
        }
    }
}
