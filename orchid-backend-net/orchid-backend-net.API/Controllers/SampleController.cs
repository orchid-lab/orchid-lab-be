using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orchid_backend_net.API.Controllers.ResponseTypes;
using orchid_backend_net.API.Dto.Sample;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Sample.Dto.Sample;
using orchid_backend_net.Application.Sample.UseCase.ChangeSampleStage;
using orchid_backend_net.Application.Sample.UseCase.CreateSampleByQuantity;
using orchid_backend_net.Application.Sample.UseCase.DestroyBecauseOfDisease;
using orchid_backend_net.Application.Sample.UseCase.GetAll;
using orchid_backend_net.Application.Sample.UseCase.GetById;
using orchid_backend_net.Application.Sample.UseCase.UpdateSampleInformation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace orchid_backend_net.API.Controllers
{
    /// <summary>
    /// sample api controller
    /// </summary>
    [Route("api/samples")]
    [ApiController]
    public class SampleController(ISender sender, ILogger<SampleController> logger) : BaseController(sender)
    {
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<SampleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSamples(
            [FromQuery] int pageNo,
            [FromQuery] int pageSize,
            [FromQuery] string experimentLogId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetAllSampleQuery(pageNo, pageSize, experimentLogId), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SampleDetailDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSampleById(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received GET request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new GetSampleByIdQuery(id), cancellationToken);
                return Ok(result);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "An error ocurred while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// technician use this api to create a numbers of sample into experiment logs
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPost]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> Create(
            [FromBody] CreateSampleForExperimentLogByQuantityCommand command, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received POST request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(command, cancellationToken);
                return Ok(result);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "An error ocurred while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// researcher use this api to change sample stage when sample is matching all requirement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut("{id}/stage")]
        [Authorize(Roles = "Researcher")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> ChangeStage(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new ChangeSampleStageCommand(id), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error ocurred while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// researcher use this api to update information of experiment log
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPut("{id}")]
        [Authorize(Roles = "Researcher")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> UpdateInformation(
            [FromRoute] string id,
            [FromBody] UpdateSampleInformationDto dto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received PUT request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new UpdateSampleInformationCommand(id, dto.Name, dto.Description, dto.Notes), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error ocurred while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// when a sample is infected, technician use this api to destroy sample of experiment log
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonResponse<string>>> Destroy(
            [FromRoute] string id,
            [FromBody] string? reason,
            CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogInformation("Received DELETE request at {Time}", DateTime.UtcNow);
                var result = await Sender.Send(new DestroySampleBecauseOfDiseaseCommand(id, reason), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error ocurred while processing the request at {Time}", DateTime.UtcNow);
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
