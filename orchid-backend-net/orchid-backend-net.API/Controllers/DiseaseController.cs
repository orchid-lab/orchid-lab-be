using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.Disease;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Disease.Dto;
using orchid_backend_net.Application.Disease.UseCase.CreateDisease;
using orchid_backend_net.Application.Disease.UseCase.DeleteDisease;
using orchid_backend_net.Application.Disease.UseCase.GetAllDisease;
using orchid_backend_net.Application.Disease.UseCase.GetDiseaseById;
using orchid_backend_net.Application.Disease.UseCase.UpdateDisease;
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

        [HttpPost]
        [Authorize(Roles = "Researcher")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateDiseaseCommand command,
            CancellationToken ct = default)
        {
            try
            {
                logger.LogInformation("POST /api/diseases at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, ct);
                return StatusCode(StatusCodes.Status201Created, new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Tạo thất bại", Detail = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Researcher")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateDiseaseRequest request,
            CancellationToken ct = default)
        {
            try
            {
                logger.LogInformation("PUT /api/diseases/{Id} at {Time}", id, DateTime.UtcNow);
                var result = await Sender.Send(
                    new UpdateDiseaseCommand(id, request.Name, request.Code, request.Description), ct);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Cập nhật thất bại", Detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Researcher")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(
            [FromRoute] int id,
            CancellationToken ct = default)
        {
            try
            {
                logger.LogInformation("DELETE /api/diseases/{Id} at {Time}", id, DateTime.UtcNow);
                var result = await Sender.Send(new DeleteDiseaseCommand(id), ct);
                return Ok(new JsonResponse<string>(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error at {Time}", DateTime.UtcNow);
                return BadRequest(new ProblemDetails { Title = "Xóa thất bại", Detail = ex.Message });
            }
        }
    }
}