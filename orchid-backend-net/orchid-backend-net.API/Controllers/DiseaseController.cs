using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.Disease;
using orchid_backend_net.Application.Disease.Dto;
using orchid_backend_net.Application.Disease.UseCase.GetAllDisease;
using orchid_backend_net.Application.Disease.UseCase.GetDiseaseById;
using orchid_backend_net.Application.Disease.UseCase.SetDiseaseActive;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.API.Controllers
{
    [Route("api/diseases")]
    [ApiController]
    public class DiseaseController(ISender sender, ILogger<DiseaseController> logger)
        : BaseController(sender)
    {
        [HttpGet]
        [ProducesResponseType(typeof(JsonResponse<IPageResult<DiseaseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNo,
            [FromQuery] int pageSize,
            CancellationToken ct = default)
        {
            try
            {
                logger.LogInformation("GET /api/diseases at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetAllDiseaseQuery(pageNo, pageSize), ct);
                return Ok(new JsonResponse<IPageResult<DiseaseDto>>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(JsonResponse<DiseaseDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(
            [FromRoute] int id,
            CancellationToken ct = default)
        {
            try
            {
                logger.LogInformation("GET /api/diseases/{Id} at {Time}", id, DateTime.UtcNow);
                var result = await Sender.Send(new GetDiseaseByIdQuery(id), ct);
                return Ok(new JsonResponse<DiseaseDetailDto>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Lấy dữ liệu thất bại", Detail = ex.Message });
            }
        }

        [HttpPatch("{id}/active")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetActive(
            [FromRoute] int id,
            [FromBody] SetDiseaseActiveRequest request,
            CancellationToken ct = default)
        {
            try
            {
                logger.LogInformation("PATCH /api/diseases/{Id}/active at {Time}", id, DateTime.UtcNow);
                var result = await Sender.Send(new SetDiseaseActiveCommand(id, request.IsActive), ct);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại", Detail = ex.Message });
            }
        }
    }
}