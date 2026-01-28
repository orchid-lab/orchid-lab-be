using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Materials.Dto;
using orchid_backend_net.Application.Materials.UseCase.GetMaterialById;
using orchid_backend_net.Application.Materials.UseCase.GetAllMaterials;
using orchid_backend_net.Application.Materials.UseCase.CreateMaterial;
using orchid_backend_net.Application.Materials.UseCase.UpdateMaterial;
using orchid_backend_net.API.Dto.Material;
using Microsoft.AspNetCore.Authorization;
using orchid_backend_net.Application.Materials.UseCase.DeleteMaterial;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// material api
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/material")]
    [ApiController]
    public class MaterialController(ISender sender, ILogger<MaterialController> logger) : BaseController(sender)
    {
        /// <summary>
        /// get all material, use for add task attribute
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<MaterialDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllChemical([FromQuery] GetAllMaterialsQuery query, CancellationToken cancellationToken = default)
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
        /// get material by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMaterialById([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var query = new GetMaterialByIdQuery() { MaterialId = id };
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
        /// create material only admin can use this
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> CreateMaterial([FromBody] CreateMaterialCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return CreatedAtAction(nameof(GetMaterialById), new { id = result }, new JsonResponse<string>("Material created successfully"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating material.");
                return BadRequest(new ProblemDetails { Title = "Tạo thất bại", Detail = ex.Message });
            }
        }
        /// <summary>
        /// update material only admin can use this
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateMaterial(
            [FromRoute] int id,
            [FromBody] UpdateMaterialDto dto, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new UpdateMaterialCommand(id, dto.Name, dto.Description, dto.Category, dto.Unit), cancellationToken);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating material.");
                return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại", Detail = ex.Message });
            }
        }

        /// <summary>
        /// delete material only admin can use this
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> DeleteMaterial(
            [FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new DeleteMaterialCommand(id), cancellationToken);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting material.");
                return BadRequest(new ProblemDetails { Title = "Xóa thất bại", Detail = ex.Message });
            }
        }
    }
}
