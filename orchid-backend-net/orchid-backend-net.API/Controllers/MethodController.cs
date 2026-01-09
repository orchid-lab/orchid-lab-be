using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.Method;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Method.Dto.Method;
using orchid_backend_net.Application.Method.UseCase.CreateMethod;
using orchid_backend_net.Application.Method.UseCase.DeleteMethod;
using orchid_backend_net.Application.Method.UseCase.GetAllMethod;
using orchid_backend_net.Application.Method.UseCase.GetMethodById;
using orchid_backend_net.Application.Method.UseCase.DeleteChemicalFromMethodStage;
using orchid_backend_net.Application.Method.UseCase.DeleteMaterialFromMethodStage;
using orchid_backend_net.Application.Method.UseCase.UpdateChemicalInMethodStage;
using orchid_backend_net.Application.Method.UseCase.UpdateMaterialInMethodStage;
using orchid_backend_net.Application.Method.UseCase.UpdateMethod;
using orchid_backend_net.Application.Method.UseCase.UpdateRequirementInMethodStage;
using orchid_backend_net.Domain.Entities;
using System.Net.Mime;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using orchid_backend_net.Application.Method.UseCase.DeleteSampleRequirementFromMethodStage;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// method api controller
    /// </summary>
    [Route("api/methods")]
    [ApiController]
    public class MethodController(ISender sender, ILogger<MethodController> logger) : BaseController(sender)
    {
        /// <summary>
        /// get all method
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<PageResult<MethodDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMethodQuery query, CancellationToken cancellationToken)
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
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// get method detail
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<MethodDetailDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetMethodByIdQuery() { Id = id }, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// create new method
        /// please fulfill every information that has provided in json
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> CreateNewMethod([FromBody] CreateMethodCommand command,  CancellationToken cancellationToken)
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
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// only using for update method information like name and description
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateMethodInformation([FromBody] UpdateMethodInformationCommand command, CancellationToken cancellationToken)
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
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// use for update method material
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="methodStageId"></param>
        /// <param name="stageMaterialId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut("{methodId}/method-stages/{methodStageId}/materials/{stageMaterialId}")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateMethodMaterial(
            int methodId,
            int methodStageId,
            string stageMaterialId,
            [FromBody] UpdateStageMaterialDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new UpdateMaterialInMethodStageCommand(methodId, methodStageId, stageMaterialId, dto.MaterialId), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// update method material in method stage
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="methodStageId"></param>
        /// <param name="stageChemicalId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut("{methodId}/method-stages/{methodStageId}/chemical/{stageChemicalId}")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateMethodChemical(
            int methodId,
            int methodStageId,
            string stageChemicalId,
            [FromBody] UpdateStageChemicalDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new UpdateChemicalInMethodStage(methodId, methodStageId, stageChemicalId, dto.ChemicalId), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// update sample requirement in method stage
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="methodStageId"></param>
        /// <param name="sampleRequirementId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut("{methodId}/method-stages/{methodStageId}/sample-requirement/{sampleRequirementId}")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateSampleRequirement(
            int methodId,
            int methodStageId,
            string sampleRequirementId,
            [FromBody] UpdateSampleRequirementDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new UpdateRequirementInMethodStageCommand(methodId, methodStageId, sampleRequirementId, dto.Minvalue, dto.MaxValue, dto.ExpectedValue), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// delete entire method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> DeleteMethod(int id, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new DeleteMethodCommand(id), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// use to remove chemical from method stage
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="methodStageId"></param>
        /// <param name="chemicalsId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpDelete("{methodId}/method-stages/{methodStageId}/chemicals/{chemicalsId}")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> DeleteMethodChemical(
            int methodId,
            int methodStageId,
            int chemicalsId,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new DeleteChemicalFromMethodStageCommand(methodId, methodStageId, chemicalsId), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// use to remove material from method stage
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="methodStageId"></param>
        /// <param name="materialId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpDelete("{methodId}/method-stages/{methodStageId}/material/{materialId}")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> DeleteMethodMaterial(
            int methodId,
            int methodStageId,
            int materialId,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new DeleteMaterialFromMethodStageCommand(methodId, methodStageId, materialId), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// use to delete sample requirement in method
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="methodStageId"></param>
        /// <param name="sampleRequirementId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpDelete("{methodId}/method-stages/{methodStageId}/sample-requirement/{sampleRequirementId}")]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        public async Task<ActionResult<JsonResponse<string>>> DeleteMethodSampleRequirement(
            int methodId,
            int methodStageId,
            string sampleRequirementId,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new DeleteSampleRequirementFromMethodStageCommand(methodId, methodStageId, sampleRequirementId), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

    }
}
