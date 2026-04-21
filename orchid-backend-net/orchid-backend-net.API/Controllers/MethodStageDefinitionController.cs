using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.Chemical;
using orchid_backend_net.Application.Chemicals.Dto;
using orchid_backend_net.Application.Chemicals.UseCase.CreateChemicals;
using orchid_backend_net.Application.Chemicals.UseCase.DeleteChemical;
using orchid_backend_net.Application.Chemicals.UseCase.GetAllChemicals;
using orchid_backend_net.Application.Chemicals.UseCase.GetChemicalById;
using orchid_backend_net.Application.Chemicals.UseCase.UpdateChemical;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.MethodStageDefinition.Dto;
using orchid_backend_net.Application.MethodStageDefinition.UseCase.CreateMethodStageDefinition;
using orchid_backend_net.Application.MethodStageDefinition.UseCase.DeleteMethodStageDefinition;
using orchid_backend_net.Application.MethodStageDefinition.UseCase.GetAllMethodStageDefinition;
using orchid_backend_net.Application.MethodStageDefinition.UseCase.GetMethodStageDefinitionById;
using orchid_backend_net.Application.MethodStageDefinition.UseCase.UpdateMethodStageDefinition;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// chemical api
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/MethodStageDefinition")]
    [ApiController]
    public class MethodStageDefinitionController(ISender sender, ILogger logger) : BaseController(sender)
    {
        /// <summary>
        /// get all method stage definition, use for method stage details
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<MethodStageDefinitionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMethodStageDefinition([FromQuery] GetAllMethodStageDefinitionQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting all method stage definition.");
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
        [ProducesResponseType(typeof(MethodStageDefinitionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMethodStageDefinitionById([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var query = new GetMethodStageDefinitionByIdQuery() { Id = id };
                var result = await Sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting method stage definition.");
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }
        
        ///// <summary>
        ///// create chemical only admin can use this
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        //public async Task<ActionResult<JsonResponse<string>>> CreateChemical([FromBody] CreateMethodStageDefinitionCommand command, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
        //        var result = await Sender.Send(command, cancellationToken);
        //        return CreatedAtAction(nameof(GetChemicalById), new { id = result }, new JsonResponse<string>(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Error occurred while creating method stage definition.");
        //        return BadRequest(new ProblemDetails { Title = "Tạo thất bại", Detail = ex.Message });
        //    }
        //}

        ///// <summary>
        ///// delete chemical only admin can use this
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //[HttpDelete("{id}")]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //public async Task<ActionResult<JsonResponse<string>>> DeleteChemical([FromRoute] int id, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
        //        var result = await Sender.Send(new DeleteMethodStageDefinitionCommand(id), cancellationToken);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Error occurred while deleting chemical.");
        //        return BadRequest(new ProblemDetails { Title = "Xóa thất bại", Detail = ex.Message });
        //    }
        //}

        ///// <summary>
        ///// update chemical only admin can use this
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="dto"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //[HttpPut("{id}")]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //public async Task<ActionResult<JsonResponse<string>>> UpdateChemical(
        //    [FromRoute] int id,
        //    [FromBody] UpdateChemicalDto dto,
        //    CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
        //        var result = await Sender.Send(new UpdateMethodStageDefinitionCommand(id, dto.Name, dto.Description), cancellationToken);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Error occurred while deleting chemical.");
        //        return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại", Detail = ex.Message });
        //    }
        //}
    }
}
